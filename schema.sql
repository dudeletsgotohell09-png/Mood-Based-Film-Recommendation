-- MoodFlick Film Recommendation Application - Database Schema
-- MySQL 8.0+

CREATE DATABASE IF NOT EXISTS mood_flick
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE mood_flick;

-- ============================================================
-- Users
-- ============================================================
CREATE TABLE IF NOT EXISTS users (
    username       VARCHAR(50)  PRIMARY KEY,
    full_name      VARCHAR(100) NOT NULL,
    password_hash  VARCHAR(255) NOT NULL,
    role           ENUM('admin', 'user') NOT NULL DEFAULT 'user',
    created_at     TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================================
-- Films
-- ============================================================
CREATE TABLE IF NOT EXISTS films (
    film_id     INT AUTO_INCREMENT PRIMARY KEY,
    title       VARCHAR(150) NOT NULL,
    year        INT NOT NULL,
    genre       VARCHAR(50) NOT NULL,
    language    VARCHAR(50) NOT NULL,
    rating      DECIMAL(3,1) DEFAULT 0.0,
    era         VARCHAR(50) NOT NULL, -- e.g., '2010s-Present', '2000s', '90\'s', '80\'s', 'Before 80\'s'
    director    VARCHAR(100),
    cast        TEXT,
    description TEXT
);

-- ============================================================
-- Mood Tags
-- ============================================================
CREATE TABLE IF NOT EXISTS mood_tags (
    mood_id   INT AUTO_INCREMENT PRIMARY KEY,
    mood_name VARCHAR(50) NOT NULL UNIQUE
);

-- ============================================================
-- Film Mood Map
-- ============================================================
CREATE TABLE IF NOT EXISTS film_mood_map (
    film_id INT NOT NULL,
    mood_id INT NOT NULL,
    PRIMARY KEY (film_id, mood_id),
    FOREIGN KEY (film_id) REFERENCES films(film_id) ON DELETE CASCADE,
    FOREIGN KEY (mood_id) REFERENCES mood_tags(mood_id) ON DELETE CASCADE
);

-- ============================================================
-- Quiz History
-- ============================================================
CREATE TABLE IF NOT EXISTS quiz_history (
    history_id        INT AUTO_INCREMENT PRIMARY KEY,
    username          VARCHAR(50) NOT NULL,
    selected_mood     VARCHAR(50) NOT NULL,
    selected_genre    VARCHAR(50) NOT NULL,
    selected_language VARCHAR(50) NOT NULL,
    selected_era      VARCHAR(50) NOT NULL,
    created_at        TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (username) REFERENCES users(username) ON DELETE CASCADE
);

-- ============================================================
-- Seed Data
-- ============================================================

-- Seed Users
INSERT INTO users (username, full_name, password_hash, role) VALUES
  ('admin', 'Administrator', SHA2('admin123', 256), 'admin'),
  ('user',  'General User',  SHA2('user123', 256),  'user')
ON DUPLICATE KEY UPDATE full_name = VALUES(full_name);

-- Seed Mood Tags
INSERT INTO mood_tags (mood_name) VALUES
  ('Happy'),
  ('Adventurous'),
  ('Romantic'),
  ('Sad'),
  ('Relaxed'),
  ('Tense')
ON DUPLICATE KEY UPDATE mood_name = VALUES(mood_name);

-- Seed Films
INSERT INTO films (film_id, title, year, genre, language, rating, era, director, cast, description) VALUES
  (1, 'Inception', 2010, 'Sci-Fi', 'English', 8.8, '2010s-Present', 'Christopher Nolan', 'Leonardo DiCaprio, Joseph Gordon-Levitt', 'A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.'),
  (2, 'The Dark Knight', 2008, 'Action', 'English', 9.0, '2000s', 'Christopher Nolan', 'Christian Bale, Heath Ledger', 'When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.'),
  (3, 'Spirited Away', 2001, 'Sci-Fi', 'Other', 8.6, '2000s', 'Hayao Miyazaki', 'Rumi Hiiragi, Miyu Irino', 'During her family\'s move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches, and spirits, and where humans are changed into beasts.'),
  (4, 'Parasite', 2019, 'Thriller', 'Korean', 8.5, '2010s-Present', 'Bong Joon Ho', 'Song Kang-ho, Lee Sun-kyun', 'Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.'),
  (5, '3 Idiots', 2009, 'Comedy', 'Hindi', 8.4, '2000s', 'Rajkumar Hirani', 'Aamir Khan, Madhavan, Mona Singh', 'Two friends are searching for their long lost companion. They revisit their college days and recall the memories of their friend who inspired them to think differently.'),
  (6, 'Amelie', 2001, 'Romantic', 'Other', 8.3, '2000s', 'Jean-Pierre Jeunet', 'Audrey Tautou, Mathieu Kassovitz', 'Amélie is an innocent and naive girl in Paris with her own sense of justice. She decides to help those around her and, along the way, discovers love.'),
  (7, 'Pulp Fiction', 1994, 'Thriller', 'English', 8.9, '90\'s', 'Quentin Tarantino', 'John Travolta, Uma Thurman, Samuel L. Jackson', 'The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.'),
  (8, 'Titanic', 1997, 'Romantic', 'English', 7.9, '90\'s', 'James Cameron', 'Leonardo DiCaprio, Kate Winslet', 'A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic.'),
  (9, 'The Shining', 1980, 'Horror', 'English', 8.4, '80\'s', 'Stanley Kubrick', 'Jack Nicholson, Shelley Duvall', 'A family heads to an isolated hotel for the winter where a sinister presence influences the father into violence, while his psychic son sees horrific forebodings from both past and future.'),
  (10, 'The Empire Strikes Back', 1980, 'Sci-Fi', 'English', 8.7, '80\'s', 'Irvin Kershner', 'Mark Hamill, Harrison Ford, Carrie Fisher', 'After the Rebels are brutally overpowered by the Empire on the ice planet Hoth, Luke Skywalker begins Jedi training with Yoda, while his friends are pursued by Darth Vader.'),
  (11, 'Back to the Future', 1985, 'Sci-Fi', 'English', 8.5, '80\'s', 'Robert Zemeckis', 'Michael J. Fox, Christopher Lloyd', 'Marty McFly, a 17-year-old high school student, is accidentally sent thirty years into the past in a time-traveling DeLorean invented by his close friend, the maverick scientist Doc Brown.'),
  (12, 'The Godfather', 1972, 'Thriller', 'English', 9.2, 'Before 80\'s', 'Francis Ford Coppola', 'Marlon Brando, Al Pacino', 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.'),
  (13, 'Cinema Paradiso', 1988, 'Comedy', 'Other', 8.5, '80\'s', 'Giuseppe Tornatore', 'Philippe Noiret, Enzo Cannavale', 'A filmmaker recalls his childhood when falling in love with the pictures at the cinema of his home village and forms a deep friendship with the cinema\'s projectionist.'),
  (14, 'Kahaani', 2012, 'Thriller', 'Hindi', 8.1, '2010s-Present', 'Sujoy Ghosh', 'Vidya Balan, Parambrata Chattopadhyay', 'A pregnant woman\'s search for her missing husband takes her from London to Kolkata, but everyone she questions denies having ever met him.'),
  (15, 'Train to Busan', 2016, 'Horror', 'Korean', 7.6, '2010s-Present', 'Sang-ho Yeon', 'Gong Yoo, Yu-mi Jung', 'While a zombie virus breaks out in South Korea, passengers struggle to survive on the train from Seoul to Busan.'),
  (16, 'A Walk to Remember', 2002, 'Romantic', 'English', 7.3, '2000s', 'Adam Shankman', 'Mandy Moore, Shane West', 'The story of two North Carolina teens, Landon Carter and Jamie Sullivan, who are thrown together after Landon gets into trouble and is made to do community service.'),
  (17, 'Aloko Udapadi', 2017, 'Action', 'Sinhala', 7.8, '2010s-Present', 'Chhatra Weeraman', 'Uddika Premarathna, Dilhani Ekanayake', 'The epic story of King Valagamba who saved the nation from foreign invaders and spearheaded writing down the Buddha\'s teachings in Pali on Ola leaves.'),
  (18, 'Machan', 2008, 'Comedy', 'Sinhala', 8.1, '2000s', 'Uberto Pasolini', 'Dharmapriya Dias, Gihan de Chickera', 'A comedy about a group of underprivileged Sri Lankan men who form a fake national handball team in order to secure visas to travel to a tournament in Germany.'),
  (19, 'Super Deluxe', 2019, 'Thriller', 'Tamil', 8.3, '2010s-Present', 'Thiagarajan Kumararaja', 'Vijay Sethupathi, Fahadh Faasil, Samantha Ruth Prabhu', 'An angry husband, a transgender woman, a priest, and a son find themselves in the most unexpected predicaments on an eventful day.')
ON DUPLICATE KEY UPDATE title = VALUES(title);

-- Seed Film Mood Map
INSERT INTO film_mood_map (film_id, mood_id) VALUES
  (1, 2), -- Inception: Adventurous
  (1, 6), -- Inception: Tense
  (2, 6), -- The Dark Knight: Tense
  (2, 2), -- The Dark Knight: Adventurous
  (3, 5), -- Spirited Away: Relaxed
  (3, 1), -- Spirited Away: Happy
  (4, 6), -- Parasite: Tense
  (4, 4), -- Parasite: Sad
  (5, 1), -- 3 Idiots: Happy
  (6, 1), -- Amelie: Happy
  (6, 3), -- Amelie: Romantic
  (7, 6), -- Pulp Fiction: Tense
  (7, 2), -- Pulp Fiction: Adventurous
  (8, 3), -- Titanic: Romantic
  (8, 4), -- Titanic: Sad
  (9, 6), -- The Shining: Tense
  (10, 2), -- Star Wars V: Adventurous
  (11, 1), -- Back to the Future: Happy
  (11, 2), -- Back to the Future: Adventurous
  (12, 6), -- The Godfather: Tense
  (12, 4), -- The Godfather: Sad
  (13, 4), -- Cinema Paradiso: Sad
  (13, 5), -- Cinema Paradiso: Relaxed
  (14, 6), -- Kahaani: Tense
  (15, 6), -- Train to Busan: Tense
  (16, 3), -- A Walk to Remember: Romantic
  (16, 4), -- A Walk to Remember: Sad
  (17, 2), -- Aloko Udapadi: Adventurous
  (17, 6), -- Aloko Udapadi: Tense
  (18, 1), -- Machan: Happy
  (19, 6)  -- Super Deluxe: Tense
ON DUPLICATE KEY UPDATE film_id = VALUES(film_id);

-- Seed initial Quiz History to generate beautiful dashboard statistics immediately
INSERT INTO quiz_history (username, selected_mood, selected_genre, selected_language, selected_era, created_at) VALUES
  ('user', 'Happy', 'Comedy', 'English', '2010s-Present', DATE_SUB(NOW(), INTERVAL 2 DAY)),
  ('user', 'Happy', 'Comedy', 'Sinhala', '2000s', DATE_SUB(NOW(), INTERVAL 5 DAY)),
  ('user', 'Adventurous', 'Action', 'English', '80\'s', DATE_SUB(NOW(), INTERVAL 12 DAY)),
  ('user', 'Tense', 'Thriller', 'English', '90\'s', DATE_SUB(NOW(), INTERVAL 18 DAY)),
  ('user', 'Relaxed', 'Sci-Fi', 'Other', '2000s', DATE_SUB(NOW(), INTERVAL 20 DAY)),
  ('user', 'Romantic', 'Romantic', 'English', '90\'s', DATE_SUB(NOW(), INTERVAL 22 DAY)),
  ('user', 'Sad', 'Romantic', 'English', '2000s', DATE_SUB(NOW(), INTERVAL 25 DAY)),
  ('user', 'Tense', 'Horror', 'Korean', '2010s-Present', DATE_SUB(NOW(), INTERVAL 26 DAY)),
  ('user', 'Adventurous', 'Sci-Fi', 'English', '2010s-Present', DATE_SUB(NOW(), INTERVAL 28 DAY))
ON DUPLICATE KEY UPDATE username = VALUES(username);

SELECT * FROM films ORDER BY film_id DESC LIMIT 5;
select * from film_mood_map;

