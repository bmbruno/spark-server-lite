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

INSERT INTO Blogs (Title, Subtitle, Content, Slug, PublishDate, AuthorID)
VALUES ('What a Great Blog Post', 'This is my subtitle.', '<p>Lorem ipsum.</p>', 'test-blog-bravo', '2023-06-30', 1);

INSERT INTO Blogs (Title, Subtitle, Content, Slug, PublishDate, AuthorID)
VALUES ('How Much Front-End Do I Need to Know?', 'It''s time to embrace JavaScript.', '<p>Sitecore XM Cloud is Sitecore''s first fully SaaS ("software as a service") product built from the ground-up and will likely be the company''s flagship CMS product in the coming year.</p>', 'test-blog-charlie', '2023-03-13', 1);

SELECT *
FROM Blogs;

--
-- BLOGTAGS
--
