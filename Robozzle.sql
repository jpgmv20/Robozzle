/* ===============================================================
   robozzle:
   - Charset utf8mb4 (suporta emojis)
   - Engine InnoDB (suporta FK e transações)
   - Comentários explicativos incluídos
   =============================================================== */

-- Cria o banco e seleciona
CREATE DATABASE IF NOT EXISTS robozzle CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE robozzle;

#--------------------------------------------------------------------------------

/* ===========================
   TABELA: users
   - informações do usuário
   - config é JSON para preferências do usuário
   ============================ */
   
CREATE TABLE IF NOT EXISTS users 
(
  id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,   
  nome VARCHAR(255) NOT NULL UNIQUE,                      
  descricao VARCHAR(512),                          
  email VARCHAR(500) NOT NULL UNIQUE,              
  password VARCHAR(80) NOT NULL,                  -- hash da senha (bcrypt)
  image_type varchar(10),						  -- tipo de imagem
  avatar_image LONGBLOB,                          -- imagem para avatar 
  config JSON,                                    -- preferências (tema, idioma, ...)
  followers_count INT UNSIGNED NOT NULL DEFAULT 0, 
  following_count INT UNSIGNED NOT NULL DEFAULT 0, 
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- criado em
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP -- atualizado em
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

#--------------------------------------------------------------------------------

/* ===========================
   TABELA: users_tokens
   - acessa as informações do usuário por token
   ============================ */

CREATE TABLE user_tokens (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    user_id BIGINT UNSIGNED NOT NULL,
    token_hash CHAR(64) NOT NULL, 
    expires_at DATETIME NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

#--------------------------------------------------------------------------------

/* ===========================
   TABELA: levels
   - cada registro é uma fase/nivel
   - level_json contém grid, start, functions, objective e constraints
   ============================ */
   
CREATE TABLE IF NOT EXISTS levels 
(
  id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  author_id BIGINT UNSIGNED NOT NULL,              -- FK para users(id)
  title VARCHAR(255) NOT NULL,                     
  descricao TEXT,                                  
  difficulty VARCHAR(50),                          
  likes_count INT UNSIGNED NOT NULL DEFAULT 0,     
  plays_count BIGINT UNSIGNED NOT NULL DEFAULT 0,  
  published BOOLEAN NOT NULL DEFAULT FALSE,        
  level_json JSON NOT NULL,                        
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_levels_author FOREIGN KEY (author_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Índices para acelerar consultas frequentes sobre níveis
CREATE INDEX idx_levels_author ON levels (author_id);
CREATE INDEX idx_levels_pub_created ON levels (published, created_at);

#--------------------------------------------------------------------------------

/* ===========================
   TABELA: programs
   - programa montado pelo jogador (sequência de chamadas de função)
   - sequence é um array JSON: ["F1","F2","F1"]
   ============================ */
   
CREATE TABLE IF NOT EXISTS programs 
(
  id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  level_id BIGINT UNSIGNED NOT NULL,  -- FK para levels(id)
  owner_id BIGINT UNSIGNED NOT NULL,  -- FK para users(id) (quem criou o programa)
  sequence JSON NOT NULL,             
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_programs_level FOREIGN KEY (level_id) REFERENCES levels(id) ON DELETE CASCADE,
  CONSTRAINT fk_programs_owner FOREIGN KEY (owner_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- índice para consultas de programas por dono
CREATE INDEX idx_programs_owner ON programs (owner_id);

#--------------------------------------------------------------------------------

/* ===========================
   TABELA: followers
   - relation directed: follower_id segue user_id
   ============================ */
   
CREATE TABLE IF NOT EXISTS followers 
(
  follower_id BIGINT UNSIGNED NOT NULL,
  user_id     BIGINT UNSIGNED NOT NULL,
  created_at  TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (follower_id, user_id), 
  INDEX idx_user_id (user_id),         -- para listar seguidores de um user
  INDEX idx_follower_id (follower_id), -- para listar quem um user segue
  CONSTRAINT fk_followers_follower FOREIGN KEY (follower_id) REFERENCES users(id) ON DELETE CASCADE,
  CONSTRAINT fk_followers_user     FOREIGN KEY (user_id)     REFERENCES users(id) ON DELETE CASCADE,
  CHECK (follower_id <> user_id)     -- evita seguir a si mesmo (MySQL 8+).
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

#--------------------------------------------------------------------------------

/* ===========================
   TABELA: likes
   - tabela N:N entre users e levels
   - PK composta impede que um usuário curta duas vezes o mesmo nível
   ============================ */
   
CREATE TABLE IF NOT EXISTS likes 
(
  user_id BIGINT UNSIGNED NOT NULL,
  level_id BIGINT UNSIGNED NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (user_id, level_id),
  INDEX idx_likes_level (level_id),
  CONSTRAINT fk_likes_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
  CONSTRAINT fk_likes_level FOREIGN KEY (level_id) REFERENCES levels(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

#--------------------------------------------------------------------------------

/* ===========================
   TABELA: comments
   - comentários deixados por usuários em níveis
   - created_at/updated_at permitem edição e ordenação
   ============================ */
   
CREATE TABLE IF NOT EXISTS comments 
(
  id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  level_id BIGINT UNSIGNED NOT NULL,
  user_id BIGINT UNSIGNED NOT NULL,
  text TEXT NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_comments_level FOREIGN KEY (level_id) REFERENCES levels(id) ON DELETE CASCADE,
  CONSTRAINT fk_comments_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- índice para buscar comentários por level
CREATE INDEX idx_comments_level ON comments (level_id);

#--------------------------------------------------------------------------------

/* ===========================
   TABELA: runs
   - histórico de execuções (runs) de programas em níveis
   ============================ */
   
CREATE TABLE IF NOT EXISTS runs 
(
  id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  level_id BIGINT UNSIGNED NOT NULL,      -- FK para levels(id)
  program_id BIGINT UNSIGNED,             -- FK para programs(id); pode ser NULL
  user_id BIGINT UNSIGNED,                -- FK para users(id); pode ser NULL (execuções anônimas)
  result ENUM('success','failure','timeout','error') NOT NULL, -- resultado final
  steps_used INT UNSIGNED NOT NULL,       -- quantos passos/ticks foram usados
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_runs_level FOREIGN KEY (level_id) REFERENCES levels(id) ON DELETE CASCADE,
  CONSTRAINT fk_runs_program FOREIGN KEY (program_id) REFERENCES programs(id) ON DELETE SET NULL,
  CONSTRAINT fk_runs_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- índices úteis para runs
CREATE INDEX idx_runs_user ON runs (user_id);
CREATE INDEX idx_runs_level ON runs (level_id);


/* ===========================
   TABELA: conversas
   - histórico de conversas
   ============================ */
CREATE TABLE IF NOT EXISTS conversations (
  id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  type ENUM('private','group') NOT NULL DEFAULT 'private',
  title VARCHAR(255) DEFAULT NULL,
  last_message_at TIMESTAMP NULL DEFAULT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS conversation_participants (
  conversation_id BIGINT UNSIGNED NOT NULL,
  user_id BIGINT UNSIGNED NOT NULL,
  joined_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (conversation_id, user_id),
  INDEX idx_conv_part_user (user_id),
  CONSTRAINT fk_conv_part_conv FOREIGN KEY (conversation_id) REFERENCES conversations(id) ON DELETE CASCADE,
  CONSTRAINT fk_conv_part_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS conversation_messages (
  id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  conversation_id BIGINT UNSIGNED NOT NULL,
  sender_id BIGINT UNSIGNED NOT NULL,
  content TEXT,
  attachment VARCHAR(1024) DEFAULT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  read_by JSON NOT NULL ,   -- array JSON de user_ids que já leram a mensagem
  CONSTRAINT fk_msg_conv FOREIGN KEY (conversation_id) REFERENCES conversations(id) ON DELETE CASCADE,
  CONSTRAINT fk_msg_user FOREIGN KEY (sender_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX idx_conv_messages_time ON conversation_messages (conversation_id, created_at DESC);


#--------------------------------------------------------------------------------

/* ===========================
		    TRIGGERS
   ============================ */

-- Remove triggers antigas se existirem
DROP TRIGGER IF EXISTS trg_followers_after_insert;
DROP TRIGGER IF EXISTS trg_followers_after_delete;
DROP TRIGGER IF EXISTS trg_like_level_after_insert;
DROP TRIGGER IF EXISTS trg_like_level_after_delete;
DROP TRIGGER IF EXISTS trg_runs_after_insert;
DROP TRIGGER IF EXISTS trg_runs_after_delete;
DROP TRIGGER IF EXISTS trg_runs_after_update;

DELIMITER $$

/* Quando alguém começa a seguir outro usuário:
   - incrementa followers_count do usuário seguido
   - incrementa following_count do seguidor
*/
CREATE TRIGGER trg_followers_after_insert
AFTER INSERT ON followers
FOR EACH ROW
BEGIN
  UPDATE users
  SET followers_count = followers_count + 1
  WHERE id = NEW.user_id;

  UPDATE users
  SET following_count = following_count + 1
  WHERE id = NEW.follower_id;
END$$

/* Quando alguém deixa de seguir:
   - decrementa os caches correspondentes (sem ficar negativo)
*/
CREATE TRIGGER trg_followers_after_delete
AFTER DELETE ON followers
FOR EACH ROW
BEGIN
  UPDATE users
  SET followers_count = GREATEST(followers_count - 1, 0)
  WHERE id = OLD.user_id;

  UPDATE users
  SET following_count = GREATEST(following_count - 1, 0)
  WHERE id = OLD.follower_id;
END$$

/* Quando um like é criado: incrementa likes_count do nivel */
CREATE TRIGGER trg_like_level_after_insert
AFTER INSERT ON likes
FOR EACH ROW
BEGIN
  UPDATE levels
  SET likes_count = likes_count + 1
  WHERE id = NEW.level_id;
END$$

/* Quando um like é removido: decrementa likes_count do nivel */
CREATE TRIGGER trg_like_level_after_delete
AFTER DELETE ON likes
FOR EACH ROW
BEGIN
  UPDATE levels
  SET likes_count = GREATEST(likes_count - 1, 0)
  WHERE id = OLD.level_id;
END$$

/* Quando uma execução (run) é criada: incrementa plays_count do level */
CREATE TRIGGER trg_runs_after_insert
AFTER INSERT ON runs
FOR EACH ROW
BEGIN
  UPDATE levels
  SET plays_count = plays_count + 1
  WHERE id = NEW.level_id;
END$$

/* Quando uma run é removida: decrementa plays_count */
CREATE TRIGGER trg_runs_after_delete
AFTER DELETE ON runs
FOR EACH ROW
BEGIN
  UPDATE levels
  SET plays_count = GREATEST(plays_count - 1, 0)
  WHERE id = OLD.level_id;
END$$

/* Quando uma run é atualizada e o level_id muda:
   - decrementa plays_count do level antigo e incrementa do novo
*/
CREATE TRIGGER trg_runs_after_update
AFTER UPDATE ON runs
FOR EACH ROW
BEGIN
  IF OLD.level_id IS NOT NULL AND NEW.level_id IS NOT NULL AND OLD.level_id <> NEW.level_id THEN
    UPDATE levels
    SET plays_count = GREATEST(plays_count - 1, 0)
    WHERE id = OLD.level_id;

    UPDATE levels
    SET plays_count = plays_count + 1
    WHERE id = NEW.level_id;
  END IF;
END$$

DELIMITER ;

#--------------------------------------------------------------------------------

/* ===========================
            PROCEDURES 
   ============================ */

-- Remove procedures antigas para evitar erro ao recriar
DROP PROCEDURE IF EXISTS create_user;
DROP PROCEDURE IF EXISTS create_follower;
DROP PROCEDURE IF EXISTS delete_follower;
DROP PROCEDURE IF EXISTS create_level;
DROP PROCEDURE IF EXISTS create_program;
DROP PROCEDURE IF EXISTS create_run;
DROP PROCEDURE IF EXISTS create_like;
DROP PROCEDURE IF EXISTS delete_like;
DROP PROCEDURE IF EXISTS create_comment;
DROP PROCEDURE IF EXISTS reconcile_counters;

DELIMITER $$

-- Cria usuário - espera password já hasheado (bcrypt)

CREATE PROCEDURE create_user (
    IN p_nome VARCHAR(255),
    IN p_descricao VARCHAR(512),
    IN p_email VARCHAR(255),
    IN p_password VARCHAR(255),
    IN p_image_type varchar(10),						  
	  IN p_avatar_image LONGBLOB,
    IN p_config JSON
)
BEGIN
    INSERT INTO users (nome, descricao, email, password, image_type, avatar_image, config)
    VALUES (p_nome, p_descricao, p_email, p_password, p_image_type, p_avatar_image, p_config);
END$$

-- Seguir (insere relação follower -> user)
CREATE PROCEDURE create_follower (
    IN p_follower_id BIGINT UNSIGNED,
    IN p_user_id BIGINT UNSIGNED
)
BEGIN
    INSERT IGNORE INTO followers (follower_id, user_id)
    VALUES (p_follower_id, p_user_id);
END$$

-- Deixar de seguir

CREATE PROCEDURE delete_follower (
    IN p_follower_id BIGINT UNSIGNED,
    IN p_user_id BIGINT UNSIGNED
)
BEGIN
    DELETE FROM followers
    WHERE follower_id = p_follower_id AND user_id = p_user_id;
END$$

-- Criar nível (level)
CREATE PROCEDURE create_level (
    IN p_author_id BIGINT UNSIGNED,
    IN p_title VARCHAR(255),
    IN p_descricao TEXT,
    IN p_difficulty VARCHAR(50),
    IN p_level_json JSON,
    IN p_published BOOLEAN
)
BEGIN
    INSERT INTO levels (author_id, title, descricao, difficulty, level_json, published)
    VALUES (p_author_id, p_title, p_descricao, p_difficulty, p_level_json, p_published);
END$$

-- Criar programa (sequência de funções)
CREATE PROCEDURE create_program (
    IN p_level_id BIGINT UNSIGNED,
    IN p_owner_id BIGINT UNSIGNED,
    IN p_sequence JSON
)
BEGIN
    INSERT INTO programs (level_id, owner_id, sequence)
    VALUES (p_level_id, p_owner_id, p_sequence);
END$$

/* Registrar execução (run)
   - passe NULL para program_id ou user_id se não houver
*/
CREATE PROCEDURE create_run (
    IN p_level_id BIGINT UNSIGNED,
    IN p_program_id BIGINT UNSIGNED,
    IN p_user_id BIGINT UNSIGNED,
    IN p_result VARCHAR(16),
    IN p_steps_used INT UNSIGNED
)
BEGIN
    INSERT INTO runs (level_id, program_id, user_id, result, steps_used)
    VALUES (p_level_id, p_program_id, p_user_id, p_result, p_steps_used);
END$$

-- Curtir nível (like)
CREATE PROCEDURE create_like (
    IN p_user_id BIGINT UNSIGNED,
    IN p_level_id BIGINT UNSIGNED
)
BEGIN
    INSERT IGNORE INTO likes (user_id, level_id)
    VALUES (p_user_id, p_level_id);
END$$

-- Remover curtida
CREATE PROCEDURE delete_like (
    IN p_user_id BIGINT UNSIGNED,
    IN p_level_id BIGINT UNSIGNED
)
BEGIN
    DELETE FROM likes WHERE user_id = p_user_id AND level_id = p_level_id;
    SELECT ROW_COUNT() AS affected;
END$$

-- Criar comentário
CREATE PROCEDURE create_comment (
    IN p_level_id BIGINT UNSIGNED,
    IN p_user_id BIGINT UNSIGNED,
    IN p_text TEXT
)
BEGIN
    INSERT INTO comments (level_id, user_id, text)
    VALUES (p_level_id, p_user_id, p_text);
END$$

/* Reconciliar contadores:
   - recalcula followers_count e following_count a partir da tabela followers
   - recalcula likes_count a partir de likes
   - recalcula plays_count a partir de runs
   Use esta procedure para manutenção periódica ou após uma operação em lote.
*/
CREATE PROCEDURE reconcile_counters()
BEGIN
  -- Recalcula followers_count
  UPDATE users u
  SET followers_count = (
    SELECT IFNULL(COUNT(*),0) FROM followers f WHERE f.user_id = u.id
  ),
  following_count = (
    SELECT IFNULL(COUNT(*),0) FROM followers f2 WHERE f2.follower_id = u.id
  );

  -- Recalcula likes_count
  UPDATE levels l
  SET likes_count = (
    SELECT IFNULL(COUNT(*),0) FROM likes li WHERE li.level_id = l.id
  );

  -- Recalcula plays_count
  UPDATE levels l2
  SET plays_count = (
    SELECT IFNULL(COUNT(*),0) FROM runs r WHERE r.level_id = l2.id
  );
END$$

DELIMITER ;

#--------------------------------------------------------------------------------

/* ===========================
            Testes
   ============================ */

-- Criar usuário
CALL create_user(
    'Pedro Miguel',
    'Criador do jogo Robozzle',
    'pedro@example.com',
    'senha123', 
    NULL,
    NULL,
    JSON_OBJECT('tema', 'dark')
);

CALL create_user(
    'Bruno Fernandes Guedes',
    'Professor de Redes e Banco de dados, e é grande amigo da 2 info',
    'guedes@example.com',
    'senha123',
    NULL,
    NULL,
    JSON_OBJECT('tema', 'dark')
);

-- Seguir
CALL create_follower(1, 2);

CALL create_follower(2, 1);

-- Deixar de seguir
CALL delete_follower(1, 2);

CALL delete_follower(2, 1);

-- Criar level
DELIMITER $$

DROP PROCEDURE IF EXISTS criar_10_fases_educativas$$

CREATE PROCEDURE criar_10_fases_educativas()
BEGIN
    -- 1. O Básico (Easy) - Apenas andar para frente
    CALL create_level(1, '01 - Primeiros Passos', 'Apenas ande para frente para coletar as estrelas.', 'Easy', '{
        "title": "01 - Primeiros Passos", "difficulty": "Easy", "grid_cell_size": 48,
        "matrix": [
            [{"color":"blue","symbol":"play","direction":1}, {"color":"blue","symbol":"star"}, {"color":"blue","symbol":"star"}, {"color":"blue","symbol":"star"}, {"color":"blue","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":3}]
    }', 1);

    -- 2. Virando (Easy) - Introdução a giros
    CALL create_level(1, '02 - A Esquina', 'Você precisará virar para pegar a última estrela.', 'Easy', '{
        "title": "02 - A Esquina", "difficulty": "Easy", "grid_cell_size": 48,
        "matrix": [
            [{"color":"green","symbol":"play","direction":1}, {"color":"green","symbol":"none"}, {"color":"green","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"green","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":4}]
    }', 1);

    -- 3. Recursão Infinita (Medium) - O conceito de chamar a si mesmo
    CALL create_level(1, '03 - O Loop', 'Use a recursão (F0 dentro de F0) para andar para sempre.', 'Medium', '{
        "title": "03 - O Loop", "difficulty": "Medium", "grid_cell_size": 48,
        "matrix": [
            [{"color":"red","symbol":"play","direction":1}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":2}]
    }', 1);

    -- 4. Condicional Simples (Medium) - Só vire no azul
    CALL create_level(1, '04 - Decisão Azul', 'Use o comando condicional (quadrado azul) para virar apenas no piso azul.', 'Medium', '{
        "title": "04 - Decisão Azul", "difficulty": "Medium", "grid_cell_size": 48,
        "matrix": [
            [{"color":"red","symbol":"play","direction":1}, {"color":"red","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"red","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"blue","symbol":"star"}, {"color":"none","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":4}]
    }', 1);

    -- 5. Escadaria (Hard) - Padrão de repetição com giros
    CALL create_level(1, '05 - Escadaria', 'Um padrão de zig-zag que se repete.', 'Hard', '{
        "title": "05 - Escadaria", "difficulty": "Hard", "grid_cell_size": 40,
        "matrix": [
            [{"color":"green","symbol":"play","direction":1}, {"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"green","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"green","symbol":"star"}, {"color":"green","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":4}]
    }', 1);

    -- 6. Duas Funções (Hard) - F0 chama F1
    CALL create_level(1, '06 - Trabalho em Equipe', 'Use F1 para definir um movimento complexo e F0 para repeti-lo.', 'Hard', '{
        "title": "06 - Trabalho em Equipe", "difficulty": "Hard", "grid_cell_size": 48,
        "matrix": [
            [{"color":"blue","symbol":"play","direction":1}, {"color":"blue","symbol":"star"}, {"color":"none","symbol":"none"}, {"color":"blue","symbol":"star"}],
            [{"color":"none","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":3}, {"name":"F1","size":3}]
    }', 1);

    -- 7. O Limitador (Hard) - Condicional para PARAR a recursão
    CALL create_level(1, '07 - O Precipício', 'Você deve usar recursão, mas precisa parar antes de cair no buraco (use a cor vermelha para parar).', 'Hard', '{
        "title": "07 - O Precipício", "difficulty": "Hard", "grid_cell_size": 48,
        "matrix": [
            [{"color":"green","symbol":"play","direction":1}, {"color":"green","symbol":"star"}, {"color":"green","symbol":"star"}, {"color":"red","symbol":"star"}, {"color":"none","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":3}]
    }', 1);

    -- 8. Troca de Cores (Insane) - Pintar o chão para criar lógica
    CALL create_level(1, '08 - Artista', 'Pinte o chão de vermelho para mudar seu próprio caminho na volta.', 'Insane', '{
        "title": "08 - Artista", "difficulty": "Insane", "grid_cell_size": 48,
        "matrix": [
            [{"color":"blue","symbol":"play","direction":1}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"star"}],
            [{"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"blue","symbol":"none"}],
            [{"color":"blue","symbol":"star"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":5}]
    }', 1);

    -- 9. O Labirinto (Insane) - Navegação complexa
    CALL create_level(1, '09 - O Labirinto', 'Navegue pelo corredor estreito usando sensores de cor.', 'Insane', '{
        "title": "09 - O Labirinto", "difficulty": "Insane", "grid_cell_size": 32,
        "matrix": [
            [{"color":"red","symbol":"play","direction":1}, {"color":"red","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"blue","symbol":"star"}],
            [{"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":6}, {"name":"F1","size":4}]
    }', 1);

    -- 10. O Chefão (Insane) - Tudo misturado
    CALL create_level(1, '10 - O Desafio Final', 'Recursão, condicionais e múltiplas funções. Boa sorte.', 'Insane', '{
        "title": "10 - O Desafio Final", "difficulty": "Insane", "grid_cell_size": 32,
        "matrix": [
            [{"color":"green","symbol":"play","direction":2}, {"color":"none","symbol":"none"}, {"color":"red","symbol":"star"}, {"color":"none","symbol":"none"}],
            [{"color":"green","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"red","symbol":"none"}],
            [{"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}],
            [{"color":"green","symbol":"star"}, {"color":"green","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"red","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":5}, {"name":"F1","size":4}, {"name":"F2","size":3}]
    }', 1);

END$$

DELIMITER ;

-- Executa a criação
CALL criar_10_fases_educativas();

DELIMITER $$

DROP PROCEDURE IF EXISTS criar_fases_11_a_20$$

CREATE PROCEDURE criar_fases_11_a_20()
BEGIN
    -- 11. Salto de Fé (Easy)
    -- Objetivo: Aprender a pular espaços vazios usando funções
    CALL create_level(1, '11 - Salto de Fé', 'Use a função para pular sobre o buraco.', 'Easy', '{
        "title": "11 - Salto de Fé", "difficulty": "Easy", "grid_cell_size": 48,
        "matrix": [
            [{"color":"blue","symbol":"play","direction":1}, {"color":"blue","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":3}]
    }', 1);

    -- 12. O Retorno (Medium)
    -- Objetivo: Ir até o fim, virar e voltar (stack de chamadas)
    CALL create_level(1, '12 - O Retorno', 'Vá até o fim do corredor e volte para o início.', 'Medium', '{
        "title": "12 - O Retorno", "difficulty": "Medium", "grid_cell_size": 48,
        "matrix": [
            [{"color":"red","symbol":"play","direction":1}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"green","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"red","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":4}, {"name":"F1","size":2}]
    }', 1);

    -- 13. Zig-Zag Colorido (Hard)
    -- Objetivo: Usar condicionais de cor para mudar o comportamento (vire na cor certa)
    CALL create_level(1, '13 - Zig-Zag Colorido', 'Vire à direita no azul e à esquerda no vermelho.', 'Hard', '{
        "title": "13 - Zig-Zag Colorido", "difficulty": "Hard", "grid_cell_size": 40,
        "matrix": [
            [{"color":"green","symbol":"play","direction":1}, {"color":"blue","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"green","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":5}]
    }', 1);

    -- 14. O Caracol (Insane)
    -- Objetivo: Espiral que exige recursão compacta
    CALL create_level(1, '14 - O Caracol', 'Entre na espiral até o centro.', 'Insane', '{
        "title": "14 - O Caracol", "difficulty": "Insane", "grid_cell_size": 32,
        "matrix": [
            [{"color":"blue","symbol":"play","direction":1}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"blue","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"green","symbol":"star"}, {"color":"red","symbol":"none"}, {"color":"blue","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"blue","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":4}, {"name":"F1","size":3}]
    }', 1);

    -- 15. Três Caminhos (Medium)
    -- Objetivo: Escolher o caminho certo baseado na cor inicial
    CALL create_level(1, '15 - Três Caminhos', 'Apenas um caminho leva à estrela. Use condicionais.', 'Medium', '{
        "title": "15 - Três Caminhos", "difficulty": "Medium", "grid_cell_size": 48,
        "matrix": [
            [{"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"green","symbol":"play","direction":1}, {"color":"green","symbol":"none"}, {"color":"blue","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"red","symbol":"star"}, {"color":"none","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":5}]
    }', 1);

    -- 16. O Loop da Morte (Hard)
    -- Objetivo: Parar a recursão antes de cair
    CALL create_level(1, '16 - O Loop da Morte', 'Pare a recursão no bloco verde antes de cair no buraco.', 'Hard', '{
        "title": "16 - O Loop da Morte", "difficulty": "Hard", "grid_cell_size": 48,
        "matrix": [
            [{"color":"blue","symbol":"play","direction":1}, {"color":"blue","symbol":"none"}, {"color":"blue","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":3}]
    }', 1);

    -- 17. Ping Pong (Hard)
    -- Objetivo: Ir e voltar várias vezes entre duas paredes
    CALL create_level(1, '17 - Ping Pong', 'Bata na parede vermelha e volte para a azul repetidamente.', 'Hard', '{
        "title": "17 - Ping Pong", "difficulty": "Hard", "grid_cell_size": 48,
        "matrix": [
            [{"color":"red","symbol":"none"}, {"color":"green","symbol":"play","direction":1}, {"color":"green","symbol":"none"}, {"color":"green","symbol":"star"}, {"color":"blue","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":4}, {"name":"F1","size":2}]
    }', 1);

    -- 18. Código Binário (Insane)
    -- Objetivo: Padrão complexo de cores 
    CALL create_level(1, '18 - Código Binário', 'Interprete a sequência de cores para navegar.', 'Insane', '{
        "title": "18 - Código Binário", "difficulty": "Insane", "grid_cell_size": 40,
        "matrix": [
            [{"color":"blue","symbol":"play","direction":2}, {"color":"none","symbol":"none"}],
            [{"color":"red","symbol":"none"}, {"color":"green","symbol":"none"}],
            [{"color":"blue","symbol":"none"}, {"color":"red","symbol":"none"}],
            [{"color":"green","symbol":"none"}, {"color":"blue","symbol":"none"}],
            [{"color":"red","symbol":"none"}, {"color":"green","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":5}, {"name":"F1","size":3}]
    }', 1);

    -- 19. Pintor Cego (Medium)
    -- Objetivo: Pintar o chão para marcar o caminho de volta
    CALL create_level(1, '19 - Pintor Cego', 'Pinte o chão para saber onde virar na volta.', 'Medium', '{
        "title": "19 - Pintor Cego", "difficulty": "Medium", "grid_cell_size": 48,
        "matrix": [
            [{"color":"none","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}],
            [{"color":"green","symbol":"play","direction":1}, {"color":"green","symbol":"none"}, {"color":"green","symbol":"star"}],
            [{"color":"none","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}]
        ],
        "functions": [{"name":"F0","size":4}]
    }', 1);

    -- 20. O Grande Desafio (Insane)
    -- Objetivo: Mapa grande com múltiplas estrelas e barreiras
    CALL create_level(1, '20 - O Grande Desafio', 'Colete todas as 3 estrelas espalhadas.', 'Insane', '{
        "title": "20 - O Grande Desafio", "difficulty": "Insane", "grid_cell_size": 32,
        "matrix": [
            [{"color":"green","symbol":"play","direction":1}, {"color":"green","symbol":"none"}, {"color":"red","symbol":"none"}, {"color":"green","symbol":"star"}, {"color":"red","symbol":"none"}],
            [{"color":"none","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"green","symbol":"none"}],
            [{"color":"blue","symbol":"star"}, {"color":"green","symbol":"none"}, {"color":"green","symbol":"none"}, {"color":"none","symbol":"none"}, {"color":"green","symbol":"star"}]
        ],
        "functions": [{"name":"F0","size":6}, {"name":"F1","size":4}, {"name":"F2","size":2}]
    }', 1);

END$$

DELIMITER ;

-- Executa a criação das fases 11 a 20
CALL criar_fases_11_a_20();


-- Criar programa
CALL create_program(
    1,
    1,
    JSON_ARRAY('F1', 'F2', 'F1')
);

-- Registrar execução
CALL create_run(
    1,
    1,
    1,
    'success',
    24
);

-- Curtir level
CALL create_like(1, 1);

-- Remover curtida
CALL delete_like(1, 1);

-- Criar comentário
CALL create_comment(1, 1, 'Muito bom esse level!');

SELECT *
FROM users;

/* UPDATE users
SET avatar_url = REPLACE(
    avatar_url,
    'gmail.com',
    'gmail.com_'
)
WHERE id = 1;
*/

-- DELETE FROM levels;
-- se precisar, resetar auto_increment
-- ALTER TABLE levels AUTO_INCREMENT = 1;

SELECT *
FROM levels;

SELECT *
FROM programs;

SELECT *
FROM runs;

SELECT *
FROM likes;

SELECT *
FROM followers;

SELECT *
FROM comments;