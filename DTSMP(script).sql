


CREATE TABLE [dbo].[tblDropdownList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DropdownValue] [varchar](100) NULL,
	[Parent] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




Create table tblFeedback_Form(Id int identity primary key,
RegionCode nvarchar(100),
RegionName nvarchar(100),
DealerCode nVarchar(100),
DealerName nvarchar(100),
Category nvarchar(100),
Languages nVarchar(100),
Models nvarchar(100),
FileTypes nvarchar(100),
Subjects nVarchar(max),
Feedback nvarchar(max),
Status bit,
AssignTo nvarchar(100),
Createdby nvarchar(100),
CreatedOn datetime,
Updatedby nvarchar(100),
UpdatedOn datetime
)



--insert into tblDropdownList values('Video','Category')
--insert into tblDropdownList values('Pdf','Category')
--insert into tblDropdownList values('other','Category')


select * from [tblDropdownList]

select * from tblFeedback_Form

sp_help tblFeedback_Form
select * from Employees


select * from tblFeedback_Form

insert into tblFeedback_Form values('123','abc','123-123-456','Maruti','CDM','Santosh','abc','M','Maruti','CDM','abc','','abc','')


--drop table tblFeedback_Form



reply 

