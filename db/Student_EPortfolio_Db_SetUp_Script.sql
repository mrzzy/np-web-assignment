/*======================================================*/
/*  Created in May 2019                                 */
/*  WEB 2019 April Semester					            */
/*  Diploma in IT/FI                                    */
/*                                                      */
/*  Database Script for setting up the database         */
/*  required for WEB Assignment.                        */
/*======================================================*/

Create Database Student_Eportfolio
GO

Use Student_Eportfolio
GO

/***************************************************************/
/***           Delete tables before creating                 ***/
/***************************************************************/

/* Table: dbo.Suggestion*/
if exists (select * from sysobjects 
  where id = object_id('dbo.Suggestion') and sysstat & 0xf = 3)
  drop table dbo.Suggestion
GO

/* Table: dbo.ProjectMember */
if exists (select * from sysobjects 
  where id = object_id('dbo.ProjectMember') and sysstat & 0xf = 3)
  drop table dbo.ProjectMember
GO

/* Table: dbo.Project */
if exists (select * from sysobjects 
  where id = object_id('dbo.Project') and sysstat & 0xf = 3)
  drop table dbo.Project
GO

/* Table: dbo.StudentSkillSet */
if exists (select * from sysobjects 
  where id = object_id('dbo.StudentSkillSet') and sysstat & 0xf = 3)
  drop table dbo.StudentSkillSet
GO

/* Table: dbo.Student */
if exists (select * from sysobjects 
  where id = object_id('dbo.Student') and sysstat & 0xf = 3)
  drop table dbo.Student
GO

/* Table: dbo.Mentor */
if exists (select * from sysobjects 
  where id = object_id('dbo.Lecturer') and sysstat & 0xf = 3)
  drop table dbo.Lecturer
GO

/* Table: dbo.SkillSet */
if exists (select * from sysobjects 
  where id = object_id('dbo.SkillSet') and sysstat & 0xf = 3)
  drop table dbo.SkillSet
GO


/***************************************************************/
/***                     Creating tables                     ***/
/***************************************************************/

/* Table: dbo.SkillSet */
CREATE TABLE dbo.SkillSet
(
  SkillSetID 			int IDENTITY (1,1),
  SkillSetName			varchar(255) 	NOT NULL,
  CONSTRAINT PK_SkillSet PRIMARY KEY NONCLUSTERED (SkillSetID)
)
GO

/* Table: dbo.Mentor */
CREATE TABLE dbo.Lecturer
(
  LecturerID			int IDENTITY (1,1),
  [Name]			    varchar(50) 	NOT NULL,
  EmailAddr		    	varchar(50)  	NOT NULL,
  [Password]		    varchar(255)  	NOT NULL DEFAULT ('p@55Lecturer'),
  [Description]			varchar(3000)	NULL,
  CONSTRAINT PK_Lecturer PRIMARY KEY NONCLUSTERED (LecturerID)
)
GO

/* Table: dbo.Student */
CREATE TABLE dbo.Student
(
  StudentID 			int IDENTITY (1,1),
  [Name]				varchar(50) 	NOT NULL,
  Course				varchar(50) 	NOT NULL,
  Photo					varchar(255) 	NULL,
  [Description]			varchar(3000) 	NULL,
  Achievement			varchar(3000) 	NULL,
  ExternalLink			varchar(255) 	NULL,
  EmailAddr		    	varchar(50)  	NOT NULL,
  [Password]			varchar(255)  	NOT NULL DEFAULT ('p@55Student'),
  MentorID 				int  			NOT NULL,
  CONSTRAINT PK_Student PRIMARY KEY NONCLUSTERED (StudentID),
  CONSTRAINT FK_Student_MentorID FOREIGN KEY (MentorID) 
  REFERENCES dbo.Lecturer(LecturerID)
)
GO

/* Table: dbo.StudentSkillSet */
CREATE TABLE dbo.StudentSkillSet
(
  StudentID				int  			NOT NULL,
  SkillSetID			int				NOT NULL,
  CONSTRAINT PK_StudentSkillSet PRIMARY KEY NONCLUSTERED (StudentID, SkillSetID),
  CONSTRAINT FK_StudentSkillSet_StudentID FOREIGN KEY (StudentID) 
  REFERENCES dbo.Student(StudentID),
  CONSTRAINT FK_StudentSkillSet_SkillSetID  FOREIGN KEY (SkillSetID) 
  REFERENCES dbo.SkillSet(SkillSetID)
)
GO

/* Table: dbo.Project */
CREATE TABLE dbo.Project
(
  ProjectID 			int IDENTITY (1,1),
  Title			    	varchar(255) 	NOT NULL,
  [Description]			varchar(3000) 	NULL,
  ProjectPoster			varchar(255) 	NULL,
  ProjectURL			varchar(255) 	NULL,
  CONSTRAINT PK_Project PRIMARY KEY NONCLUSTERED (ProjectID)
)
GO

/* Table: dbo.ProjectMember */
CREATE TABLE dbo.ProjectMember
(
  ProjectID				int				NOT NULL,
  StudentID				int  			NOT NULL,
  [Role]				varchar(50)  	NOT NULL DEFAULT ('Member') CHECK ([Role] IN ('Leader','Member')),
  CONSTRAINT PK_ProjectMember PRIMARY KEY NONCLUSTERED (ProjectID, StudentID),
  CONSTRAINT FK_ProjectMember_ProjectID FOREIGN KEY (ProjectID) 
  REFERENCES dbo.Project(ProjectID),
  CONSTRAINT FK_ProjectMember_StudentID  FOREIGN KEY (StudentID) 
  REFERENCES dbo.Student(StudentID)
)
GO

/* Table: dbo.Suggestion */
CREATE TABLE dbo.Suggestion
(
  SuggestionID 			int IDENTITY (1,1),
  LecturerID			int 	        NOT NULL,
  StudentID				int				NOT NULL,
  [Description]			varchar(3000) 	NULL,
  [Status]				char(1) 		NOT NULL DEFAULT ('N') CHECK ([Status] IN ('Y','N')),  
  DateCreated			datetime		NOT NULL DEFAULT (getdate()),
  CONSTRAINT PK_Suggestion PRIMARY KEY NONCLUSTERED (SuggestionID),
  CONSTRAINT FK_Suggestion_LecturerID FOREIGN KEY (LecturerID) 
  REFERENCES dbo.Lecturer(LecturerID),
  CONSTRAINT FK_Suggestion_StudentID FOREIGN KEY (StudentID) 
  REFERENCES dbo.Student(StudentID)
)
GO


/***************************************************************/
/***                Populate Sample Data                     ***/
/***************************************************************/

SET IDENTITY_INSERT [dbo].[SkillSet] ON 
INSERT [dbo].[SkillSet] ([SkillSetID], [SkillSetName]) VALUES (1, 'Python Programming')
INSERT [dbo].[SkillSet] ([SkillSetID], [SkillSetName]) VALUES (2, 'Web Design')
INSERT [dbo].[SkillSet] ([SkillSetID], [SkillSetName]) VALUES (3, 'Database Design')
INSERT [dbo].[SkillSet] ([SkillSetID], [SkillSetName]) VALUES (4, 'ASP.NET')
INSERT [dbo].[SkillSet] ([SkillSetID], [SkillSetName]) VALUES (5, 'Android Programming')
INSERT [dbo].[SkillSet] ([SkillSetID], [SkillSetName]) VALUES (6, 'iOS Programming')
INSERT [dbo].[SkillSet] ([SkillSetID], [SkillSetName]) VALUES (7, 'SAP ERP Solutions')
INSERT [dbo].[SkillSet] ([SkillSetID], [SkillSetName]) VALUES (8, 'Network Configuration')
SET IDENTITY_INSERT [dbo].[SkillSet] OFF

SET IDENTITY_INSERT [dbo].[Lecturer] ON 
INSERT [dbo].[Lecturer] ([LecturerID], [Name], [EmailAddr], [Password], [Description]) VALUES (1, 'Peter Ghim', 'Peter_Ghim@ap.edu.sg', 'p@55Mentor', 'Programming')
INSERT [dbo].[Lecturer] ([LecturerID], [Name], [EmailAddr], [Password], [Description]) VALUES (2, 'Lisa Lee', 'Lisa_Lee@ap.edu.sg', 'p@55Mentor', 'UX Design')
SET IDENTITY_INSERT [dbo].[Lecturer] OFF

SET IDENTITY_INSERT [dbo].[Student] ON 
INSERT [dbo].[Student] ([StudentID], [Name], [Course], [Photo], [Description], [Achievement], [ExternalLink], [EmailAddr], [Password], [MentorID]) VALUES (1, 'Shabbir Mustaffa', 'IT', 'male1.jpg', 'I am a passionate individual with a proven track record in creating database and cloud projects. I have strong technical skills as well as excellent interpersonal skills which were further honed during my time in school and internship. I am also an individual who is eager to be challenged in order to grow and improve my career development so that I will be better equipped to handle challenging projects in the future.', 'Director''s list in AY 2015/16',  'https://www.linkedin.com/in/ShabbirMustaffa', 's1234111@ap.edu.sg',  'p@55Student', 1)
INSERT [dbo].[Student] ([StudentID], [Name], [Course], [Photo], [Description], [Achievement], [ExternalLink], [EmailAddr], [Password], [MentorID]) VALUES (2, 'Amy Ng', 'IT', 'female1.jpg', 'I am interested in set up on my own business in future, that''s why I choose specialized in Business Management. I am willing to learn and upgrade oneself in different environment and able to work both independently and in a team.', 'Best Performance in the module "Communication & Contemporary Issues" AY 2015/16',  'https://www.linkedin.com/in/AmyNg96', 's1234112@ap.edu.sg',  'p@55Student', 1)
INSERT [dbo].[Student] ([StudentID], [Name], [Course], [Photo], [Description], [Achievement], [ExternalLink], [EmailAddr], [Password], [MentorID]) VALUES (3, 'Raymond Smith', 'IT', 'male3.jpg', 'I have always had a passion for computers and technology and this passion is what drives me every day to want to learn more about it. I love working and collaborating with people and always believe in helping one another to excel to bring out the best in everyone. I am always eager to pick up new challenges to continue developing and growing my skill set to be able to integrate what I have learnt into my work or daily life.', 'Most Outstanding Student AY 2015/16 Sem 2',  'https://www.linkedin.com/in/raySmith94', 's1234113@ap.edu.sg',  'p@55Student', 1)
INSERT [dbo].[Student] ([StudentID], [Name], [Course], [Photo], [Description], [Achievement], [ExternalLink], [EmailAddr], [Password], [MentorID]) VALUES (4, 'John Tan', 'FI', 'male2.jpg', 'I am an aspiring financial analyst who has the skills to prepare dashboards and summaries of large amounts of data. Having a background of an IT savvy parent has helped in growing my passion for coding, having dabbled in Java and Python as some examples. Along with the teachings of my course of study with Tableau and analytical skills that will help in being a Financial Analyst.', '3rd prize in Singapore Geo-spatial Challenge Business Analytics Mania (2016)',  'https://www.linkedin.com/in/geetha', 's1234114@ap.edu.sg',  'p@55Student', 2)
INSERT [dbo].[Student] ([StudentID], [Name], [Course], [Photo], [Description], [Achievement], [ExternalLink], [EmailAddr], [Password], [MentorID]) VALUES (5, 'Geetha S', 'FI', 'female2.jpg', 'I am a self-motivated and independent individual who strive for excellence.', NULL,  'https://www.linkedin.com/in/johntan97', 's1234115@ap.edu.sg',  'p@55Student', 2)
SET IDENTITY_INSERT [dbo].[Student] OFF

INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (1,1)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (1,3)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (1,4)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (1,8)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (2,2)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (2,5)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (3,2)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (3,3)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (3,4)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (3,6)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (4,1)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (4,3)
INSERT [dbo].[StudentSkillSet] ([StudentID], [SkillSetID]) VALUES (4,7)

SET IDENTITY_INSERT [dbo].[Project] ON
INSERT [dbo].[Project] ([ProjectID], [Title], [Description], [ProjectPoster], [ProjectURL]) VALUES (1, 'Cipher', 'Cipher, is a platformer fighting game developed within 4 weeks. The game''s playable characters are created with a unique fighting style. The gameplay objective differs from those traditional fighters, by aiming to knock opponents off of the stage instead of depleting life bars. Instead of using physical combats to knock off the enemies, the characters are to use magical powers to shoot at their enemies, giving each character different style of fighting, different strategies and unique sets of moves.', 'Project_1_Poster.jpg', 'https://www.myweb.com/cipher')
INSERT [dbo].[Project] ([ProjectID], [Title], [Description], [ProjectPoster], [ProjectURL]) VALUES (2, 'Runner - Bring The Shop Closer To You', 'Runner is a mobile application that bring the shop closer to you - Order through our mobile application - Runner from nearby will be able to see your order request - Runner will help you purchase your order - Delivery to your house within 30 minutes With the constraint of distance, your order will be serviced by the people nearby and delivery will be achieved within 30 minutes.', 'Project_2_Poster.jpg', 'https://www.youtube.com/watch?v=runner')
INSERT [dbo].[Project] ([ProjectID], [Title], [Description], [ProjectPoster], [ProjectURL]) VALUES (3, 'SMU Internship Project', 'We write demo applications in Java, JavaScript, Python and C# for all APIs, and documented down how to call the APIs. Then, we create our own web application based on the APIs.', 'Project_3_Poster.jpg', 'https://www.smu.edu.sg/myWebApp')
INSERT [dbo].[Project] ([ProjectID], [Title], [Description], [ProjectPoster], [ProjectURL]) VALUES (4, 'Project Ubin', 'This project is led by Monetary Authority of Singapore (MAS), Association of Banks in Singapore (ABS) and powered by Accenture. Partnering with 11 banks, Project Ubin discovers the use of Distributed Ledger Technology (DLT) for clearing and settlement of payment and securities. The goal is to get a better understanding of DLT and the feasibility of developing more resilient and efficient alternatives to today''s financial market operations and systems.', 'Project_4_Poster.jpg', 'https://www.youtube.com/watch?v=projubi')
SET IDENTITY_INSERT [dbo].[Project] OFF

INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (1, 3, 'Leader')
INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (1, 1, 'Member')
INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (2, 2, 'Leader')
INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (2, 5, 'Member')
INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (3, 4, 'Leader')
INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (3, 3, 'Member')
INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (3, 1, 'Member')
INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (4, 5, 'Leader')
INSERT [dbo].[ProjectMember] ([ProjectID], [StudentID], [Role]) VALUES (4, 2, 'Member')

SET IDENTITY_INSERT [dbo].[Suggestion] ON
INSERT [dbo].[Suggestion] ([SuggestionID], [LecturerID], [StudentID], [Description], [Status], [DateCreated]) VALUES (1, 1, 2, 'Hi Amy, You should specify the skills that you have learnt from the course.', 'Y', '25-Apr-2019')
INSERT [dbo].[Suggestion] ([SuggestionID], [LecturerID], [StudentID], [Description], [Status], [DateCreated]) VALUES (2, 2, 5, 'Hi Geetha, You should write more details about yourself, highlight your achievements and specify the skills that you have learnt.', 'N', '02-May-2019')
SET IDENTITY_INSERT [dbo].[Suggestion] OFF

