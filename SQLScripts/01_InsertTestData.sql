--
-- AUTHORS
--

INSERT INTO Authors (FirstName, LastName, Email)
VALUES ('Brandon', 'Bruno', 'test1@test.com');

INSERT INTO Authors (FirstName, LastName, Email)
VALUES ('Laura', 'Kolpien', 'test2@test.com');

INSERT INTO Authors (FirstName, LastName, Email)
VALUES ('Lizzie', 'The Dog', 'test3@test.com');

SELECT *
FROM Authors;

--
-- BLOGS
--

INSERT INTO Blogs (Title, Subtitle, Content, Slug, PublishDate, AuthorID)
VALUES ('Test Title', 'Subtitle!!!', '<p>Hello, world!</p>', 'test-blog-alpha', '2023-07-25', 1);

SELECT *
FROM Blogs;

--
-- BLOGTAGS
--
