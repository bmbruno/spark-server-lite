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

/*
INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('Test Title', 'Subtitle!!!', '<p>Hello, world!</p>', 'test-blog-alpha', '2023-07-25', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('What a Great Blog Post', 'This is my subtitle.', '<p>Lorem ipsum.</p>', 'test-blog-bravo', '2023-06-30', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('How Much Front-End Do I Need to Know? (1)', 'It''s time to embrace JavaScript.', '<p>Sitecore XM Cloud is Sitecore''s first fully SaaS ("software as a service") product built from the ground-up and will likely be the company''s flagship CMS product in the coming year.</p>', 'test-blog-charlie', '2023-03-13', 1);
*/

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, ImagePath, ImageThumbnailPath, PublishDate, AuthorID)
VALUES ('Blog Title 1', 'Subtitle!!!', 'Hello, World!', '<p>Hello, world!</p>', 'test-blog-one', '/images/blog-banners/01.jpg', '/images/blog-banners/01-thumb.jpg', '2023-07-31', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, ImagePath, ImageThumbnailPath, PublishDate, AuthorID)
VALUES ('Blog Title 2', 'Subtitle!!!', 'Hello, World!', '<p>Hello, world!</p>', 'test-blog-two', '/images/blog-banners/02.jpg', '/images/blog-banners/02-thumb.jpg', '2023-07-15', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('Blog Title 3', 'Subtitle!!!', 'Hello, World!', '<p>Hello, world!</p>', 'test-blog-three', '2023-06-15', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('Blog Title 4', 'Subtitle!!!', 'Hello, World!', '<p>Hello, world!</p>', 'test-blog-four', '2023-05-15', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('Blog Title 5', 'Subtitle!!!', 'Hello, World!', '<p>Hello, world!</p>', 'test-blog-five', '2023-04-15', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('Blog Title 6', 'Subtitle!!!', 'Hello, World!', '<p>Hello, world!</p>', 'test-blog-six', '2023-03-15', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('Blog Title 7', 'Subtitle!!!', 'Hello, World!', '<p>Hello, world!</p>', 'test-blog-seven', '2023-02-15', 1);

INSERT INTO Blogs (Title, Subtitle, Markdown, Content, Slug, PublishDate, AuthorID)
VALUES ('Blog Title 8', 'Subtitle!!!', 'Hello, World!','<p>Hello, world!</p>', 'test-blog-eight', '2023-01-15', 1);

SELECT *
FROM Blogs;

--
-- BLOGTAGS
--

INSERT INTO BlogTags (Name, Active, CreateDate)
VALUES ('Tag Alpha', 1, date('now'));

INSERT INTO BlogTags (Name, Active, CreateDate)
VALUES ('Tag Bravo', 1, date('now'));

INSERT INTO BlogTags (Name, Active, CreateDate)
VALUES ('Tag Charlie', 1, date('now'));

INSERT INTO BlogTags (Name, Active, CreateDate)
VALUES ('Tag Delta', 1, date('now'));

INSERT INTO BlogTags (Name, Active, CreateDate)
VALUES ('Tag Echo', 1, date('now'));

SELECT *
FROM BlogTags;

--
-- BlogsToTags
--

INSERT INTO BlogsToTags (BlogID, BlogTagID) VALUES (1, 1);
INSERT INTO BlogsToTags (BlogID, BlogTagID) VALUES (1, 2);
INSERT INTO BlogsToTags (BlogID, BlogTagID) VALUES (1, 3);
INSERT INTO BlogsToTags (BlogID, BlogTagID) VALUES (2, 4);
INSERT INTO BlogsToTags (BlogID, BlogTagID) VALUES (2, 5);

SELECT *
FROM BlogsToTags;