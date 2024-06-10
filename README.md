# VolunteerHub

## Описание
Этот проект предназначен для управления деятельностью волонтёрских организаций. Он позволяет администраторам эффективно координировать задачи, отслеживать прогресс и взаимодействовать с волонтёрами.

## Начало работы

### Предварительные требования
- MS SQL Server
- .NET Framework
- Visual Studio

### Установка
1. Создайте базу данных MS SQL, используя предоставленный скрипт.
2. Откройте файл `App.config`.
3. Найдите строку `<connectionStrings>` и измените содержимое следующей части кода на адрес вашей базы данных.
   Пример: `data source=ваш_адрес_базы_данных;initial catalog=ваша_база_данных;integrated security=True;`
   По умолчанию: catalog=dbVolunteerHub
   Альтернативный вариант подключения, при разворачивании на сервере с аутентификацией: data source=ваш_адрес_базы_данных;initial catalog=ваша_база_данных;user id=ваше_имя_пользователя;password=ваш_пароль;trustservercertificate=True
4. Соберите проект.

## Использование
Для входа в систему используйте следующие учетные данные администратора:
- **Логин:** Admin
- **Пароль:** Admin

### Скрипт для разворачивания БД
create database [dbVolunteerHub];
use [dbVolunteerHub];
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administrators](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[SpecialPermissions] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentBlocks](
	[ContentBlockID] [int] IDENTITY(1,1) NOT NULL,
	[NewsID] [int] NULL,
	[ContentType] [nvarchar](50) NULL,
	[Content] [nvarchar](max) NULL,
	[OrderIndex] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ContentBlockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventApplications](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[EventProposal] [nvarchar](max) NULL,
	[ApplicationDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[TitleEvent] [nvarchar](255) NULL,
	[AddressEvent] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventRegistrations](
	[RegistrationID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NULL,
	[UserID] [int] NULL,
	[TimePreference] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RegistrationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[EventDate] [datetime] NULL,
	[Location] [nvarchar](255) NULL,
	[OrganizerID] [int] NULL,
	[RequiredVolunteers] [int] NULL,
	[PicEvent] [varbinary](max) NULL,
	[Image] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genders](
	[GenderID] [int] IDENTITY(1,1) NOT NULL,
	[GenderName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[GenderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[ImageID] [int] IDENTITY(1,1) NOT NULL,
	[ImageData] [varbinary](max) NOT NULL,
	[UploadDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[NewsID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[PublicationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[NewsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsImages](
	[NewsImageID] [int] IDENTITY(1,1) NOT NULL,
	[ContentBlockID] [int] NULL,
	[ImageID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[NewsImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Message] [nvarchar](max) NULL,
	[NotificationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NULL,
	[UserID] [int] NULL,
	[Rating] [int] NULL,
	[Comment] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestAnswers](
	[AnswerID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NOT NULL,
	[AnswerText] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AnswerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestQuestions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionText] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestResultAnswers](
	[ResultAnswerID] [int] IDENTITY(1,1) NOT NULL,
	[ResultID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[AnswerID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultAnswerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestResults](
	[ResultID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[TestDate] [datetime] NULL,
	[ResultType] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[Skills] [nvarchar](max) NULL,
	[Preferences] [nvarchar](max) NULL,
	[ContactInfo] [nvarchar](max) NULL,
	[Firstname] [nvarchar](100) NULL,
	[Lastname] [nvarchar](100) NULL,
	[RoleID] [int] NULL,
	[GenderID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[EventApplications] ON 

INSERT [dbo].[EventApplications] ([ApplicationID], [UserID], [EventProposal], [ApplicationDate], [Status], [TitleEvent], [AddressEvent]) VALUES (1, 1, N'Мероприятия', CAST(N'2024-05-18T00:00:00.000' AS DateTime), N'Proposed', N'Название', N'Адрес')
SET IDENTITY_INSERT [dbo].[EventApplications] OFF

SET IDENTITY_INSERT [dbo].[Genders] ON 

INSERT [dbo].[Genders] ([GenderID], [GenderName]) VALUES (1, N'Мужской')
INSERT [dbo].[Genders] ([GenderID], [GenderName]) VALUES (2, N'Женский')
SET IDENTITY_INSERT [dbo].[Genders] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Администратор')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Пользователь')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'Гость')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[TestAnswers] ON 

INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (1, 1, N'События и мероприятия')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (2, 1, N'Охрана окружающей среды')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (3, 1, N'Спорт и физическая активность')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (4, 1, N'Патриотизм и гражданственность')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (5, 1, N'Медиа и коммуникации')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (6, 2, N'Организация и планирование мероприятий')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (7, 2, N'Работа с детьми или пожилыми людьми')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (8, 2, N'Знание социальных сетей или цифровых технологий')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (9, 2, N'Физическая выносливость')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (10, 2, N'Умение работать в команде')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (11, 3, N'Помощь другим')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (12, 3, N'Приобретение новых навыков')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (13, 3, N'Создание связей')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (14, 3, N'Получение опыта работы')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (15, 3, N'Внесение вклада в свое сообщество')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (16, 4, N'Несколько часов в неделю')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (17, 4, N'Несколько дней в месяц')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (18, 4, N'По мере необходимости')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (19, 5, N'Гибкий')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (20, 5, N'Фиксированный')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (21, 5, N'Сезонный')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (22, 6, N'Самостоятельно')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (23, 6, N'В команде')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (24, 6, N'И то, и другое')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (25, 7, N'Планирование и проведение мероприятий')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (26, 7, N'Уборка парков или пляжей')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (27, 7, N'Поддержка спортивных команд')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (28, 7, N'Работа с ветеранами или военнослужащими')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (29, 7, N'Создание контента для социальных сетей')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (30, 8, N'Да')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (31, 8, N'Нет')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (32, 8, N'Не имеет значения')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (33, 9, N'Да')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (34, 9, N'Нет')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (35, 9, N'Не имеет значения')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (36, 10, N'Да')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (37, 10, N'Нет')
INSERT [dbo].[TestAnswers] ([AnswerID], [QuestionID], [AnswerText]) VALUES (38, 10, N'Не имеет значения')
SET IDENTITY_INSERT [dbo].[TestAnswers] OFF
GO
SET IDENTITY_INSERT [dbo].[TestQuestions] ON 

INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (1, N'Какая из следующих тем вас больше всего интересует?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (2, N'Какими навыками вы обладаете?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (3, N'Что вас мотивирует заниматься волонтерством?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (4, N'Сколько времени вы готовы уделять волонтерству?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (5, N'Какой тип рабочего графика вам больше всего подходит?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (6, N'Вы предпочитаете работать самостоятельно или в команде?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (7, N'Какая из следующих задач кажется вам наиболее интересной?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (8, N'Вы готовы работать на открытом воздухе?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (9, N'Вы готовы работать с детьми или пожилыми людьми?')
INSERT [dbo].[TestQuestions] ([QuestionID], [QuestionText]) VALUES (10, N'Вы готовы работать в условиях стресса или под давлением?')
SET IDENTITY_INSERT [dbo].[TestQuestions] OFF
GO

SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Email], [Skills], [Preferences], [ContactInfo], [Firstname], [Lastname], [RoleID], [GenderID]) VALUES (1, N'Admin', N'Admin', N'lasck@mail.ru', N'Коммуникативность, Лидерство', N'Природа', N'hash1', N'Алексей', N'Волков', 1, 1)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Email], [Skills], [Preferences], [ContactInfo], [Firstname], [Lastname], [RoleID], [GenderID]) VALUES (2, N'Vlad', N'2', N'mal@list.ru', N'Писательство, Речь', N'Виртуальное волонтерство', N'2', N'Владислав', N'Пушко', NULL, 1)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Email], [Skills], [Preferences], [ContactInfo], [Firstname], [Lastname], [RoleID], [GenderID]) VALUES (3, N'Bob', N'hash3', N'bob@example.com', N'Организация мероприятий', N'Небольшие проекты', N'hash3', N'Боб', N'Михайлов', NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO

SET IDENTITY_INSERT [dbo].[TestResultAnswers] ON 

INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (1, 5, 1, 1)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (2, 5, 2, 7)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (3, 5, 3, 13)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (4, 5, 4, 18)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (5, 5, 5, 20)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (6, 5, 6, 24)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (7, 5, 7, 27)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (8, 5, 8, 32)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (9, 5, 9, 35)
INSERT [dbo].[TestResultAnswers] ([ResultAnswerID], [ResultID], [QuestionID], [AnswerID]) VALUES (10, 5, 10, 37)
SET IDENTITY_INSERT [dbo].[TestResultAnswers] OFF
GO
SET IDENTITY_INSERT [dbo].[TestResults] ON 

INSERT [dbo].[TestResults] ([ResultID], [UserID], [TestDate], [ResultType]) VALUES (1, NULL, NULL, N'Example result type')
INSERT [dbo].[TestResults] ([ResultID], [UserID], [TestDate], [ResultType]) VALUES (2, NULL, NULL, N'Example result type')
INSERT [dbo].[TestResults] ([ResultID], [UserID], [TestDate], [ResultType]) VALUES (3, NULL, NULL, N'Тип результата по умолчанию')
INSERT [dbo].[TestResults] ([ResultID], [UserID], [TestDate], [ResultType]) VALUES (4, 2, CAST(N'2024-05-24T23:57:40.013' AS DateTime), N'Тип результата по умолчанию')
INSERT [dbo].[TestResults] ([ResultID], [UserID], [TestDate], [ResultType]) VALUES (5, 2, CAST(N'2024-05-25T00:15:53.187' AS DateTime), N'Тип результата по умолчанию')
SET IDENTITY_INSERT [dbo].[TestResults] OFF
GO

ALTER TABLE [dbo].[Images] ADD  DEFAULT (getdate()) FOR [UploadDate]
GO
ALTER TABLE [dbo].[TestResults] ADD  DEFAULT (getdate()) FOR [TestDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_GenderID]  DEFAULT (NULL) FOR [GenderID]
GO
ALTER TABLE [dbo].[Administrators]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ContentBlocks] WITH CHECK ADD FOREIGN KEY([NewsID])
REFERENCES [dbo].[News] ([NewsID])
GO
ALTER TABLE [dbo].[EventApplications]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[EventRegistrations]  WITH CHECK ADD FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[EventRegistrations]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD FOREIGN KEY([OrganizerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[NewsImages]  WITH CHECK ADD FOREIGN KEY([ContentBlockID])
REFERENCES [dbo].[ContentBlocks] ([ContentBlockID])
GO
ALTER TABLE [dbo].[NewsImages]  WITH CHECK ADD FOREIGN KEY([ImageID])
REFERENCES [dbo].[Images] ([ImageID])
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TestAnswers]  WITH CHECK ADD FOREIGN KEY([QuestionID])
REFERENCES [dbo].[TestQuestions] ([QuestionID])
GO
ALTER TABLE [dbo].[TestResultAnswers]  WITH CHECK ADD FOREIGN KEY([AnswerID])
REFERENCES [dbo].[TestAnswers] ([AnswerID])
GO
ALTER TABLE [dbo].[TestResultAnswers]  WITH CHECK ADD FOREIGN KEY([QuestionID])
REFERENCES [dbo].[TestQuestions] ([QuestionID])
GO
ALTER TABLE [dbo].[TestResultAnswers]  WITH CHECK ADD FOREIGN KEY([ResultID])
REFERENCES [dbo].[TestResults] ([ResultID])
GO
ALTER TABLE [dbo].[TestResults]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Genders] FOREIGN KEY([GenderID])
REFERENCES [dbo].[Genders] ([GenderID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Genders]
GO
