using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DynamicSiteService.Migrations
{
    public partial class updatev2l : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lang",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ServiceName = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    UrlTarget = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceConfig_ServiceConfig_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ServiceConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiteConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    StartPage = table.Column<string>(nullable: false),
                    StartAction = table.Column<string>(nullable: false),
                    version = table.Column<string>(nullable: false),
                    layoutID = table.Column<string>(nullable: false),
                    layoutUrlBase = table.Column<string>(nullable: false),
                    layoutUrl = table.Column<string>(nullable: false),
                    Logo = table.Column<string>(nullable: false),
                    Map = table.Column<string>(nullable: true),
                    DefaultImage = table.Column<string>(nullable: true),
                    BaseUrl = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    JokerPass = table.Column<string>(nullable: false),
                    MetaKeywords = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    Adress = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    MailGorunenAd = table.Column<string>(nullable: true),
                    SmtpHost = table.Column<string>(nullable: true),
                    SmtpPort = table.Column<string>(nullable: true),
                    SmtpMail = table.Column<string>(nullable: true),
                    SmtpMailPass = table.Column<string>(nullable: true),
                    SmtpSSL = table.Column<bool>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Youtube = table.Column<string>(nullable: true),
                    GooglePlus = table.Column<string>(nullable: true),
                    Tumblr = table.Column<string>(nullable: true),
                    HeadScript = table.Column<string>(nullable: true),
                    HeadStyle = table.Column<string>(nullable: true),
                    BodyScript = table.Column<string>(nullable: true),
                    FooterScript = table.Column<string>(nullable: true),
                    FooterStyle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    Pass = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    Mail1 = table.Column<string>(nullable: true),
                    Mail2 = table.Column<string>(nullable: true),
                    Phone1 = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    Phone3 = table.Column<string>(nullable: true),
                    Adress1 = table.Column<string>(nullable: true),
                    Adress2 = table.Column<string>(nullable: true),
                    BirdhDay = table.Column<DateTime>(nullable: false),
                    UserNo = table.Column<string>(nullable: true),
                    SexType = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    ProfilImage = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    IsRead = table.Column<string>(nullable: true),
                    FormTypeId = table.Column<int>(nullable: false),
                    Custom1 = table.Column<string>(nullable: true),
                    Custom2 = table.Column<string>(nullable: true),
                    Custom3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_FormType_FormTypeId",
                        column: x => x.FormTypeId,
                        principalTable: "FormType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    ContentPageId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    TemplateType = table.Column<int>(nullable: false),
                    ContentTypesId = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: false),
                    ExternalLink = table.Column<string>(nullable: true),
                    BannerText = table.Column<string>(nullable: true),
                    BannerButtonText = table.Column<string>(nullable: true),
                    ButtonText = table.Column<string>(nullable: true),
                    ButtonLink = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ContentShort = table.Column<string>(nullable: true),
                    ContentData = table.Column<string>(nullable: true),
                    VideoLink = table.Column<string>(nullable: true),
                    IsHeaderMenu = table.Column<bool>(nullable: true),
                    IsFooterMenu = table.Column<bool>(nullable: true),
                    IsClick = table.Column<bool>(nullable: true),
                    IsSideMenu = table.Column<bool>(nullable: true),
                    IsHamburgerMenu = table.Column<bool>(nullable: true),
                    FormTypeId = table.Column<int>(nullable: true),
                    IsGallery = table.Column<bool>(nullable: true),
                    IsMap = table.Column<bool>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    IsPublish = table.Column<bool>(nullable: true),
                    ContentOrderNo = table.Column<int>(nullable: true),
                    MetaTitle = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    OrjId = table.Column<int>(nullable: true),
                    OrjId1 = table.Column<int>(nullable: true),
                    LangId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentPage_ContentPage_ContentPageId",
                        column: x => x.ContentPageId,
                        principalTable: "ContentPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentPage_ContentTypes_ContentTypesId",
                        column: x => x.ContentTypesId,
                        principalTable: "ContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentPage_FormType_FormTypeId",
                        column: x => x.FormTypeId,
                        principalTable: "FormType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentPage_Lang_LangId",
                        column: x => x.LangId,
                        principalTable: "Lang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentPage_ContentPage_OrjId1",
                        column: x => x.OrjId1,
                        principalTable: "ContentPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Spec",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SpecType = table.Column<int>(nullable: false),
                    OrjId = table.Column<int>(nullable: true),
                    IsTanim = table.Column<bool>(nullable: true),
                    LangId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spec", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spec_Lang_LangId",
                        column: x => x.LangId,
                        principalTable: "Lang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Spec_Spec_OrjId",
                        column: x => x.OrjId,
                        principalTable: "Spec",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Spec_Spec_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Spec",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    RoleParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    ServiceConfigId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Role_RoleParentId",
                        column: x => x.RoleParentId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_ServiceConfig_ServiceConfigId",
                        column: x => x.ServiceConfigId,
                        principalTable: "ServiceConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    Types = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Guid = table.Column<string>(nullable: true),
                    Alt = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    data_class = table.Column<string>(nullable: true),
                    DocumentId = table.Column<int>(nullable: true),
                    GalleryId = table.Column<int>(nullable: true),
                    ThumbImageId = table.Column<int>(nullable: true),
                    BannerImageId = table.Column<int>(nullable: true),
                    PictureId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_ContentPage_BannerImageId",
                        column: x => x.BannerImageId,
                        principalTable: "ContentPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_ContentPage_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "ContentPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_ContentPage_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "ContentPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_ContentPage_PictureId",
                        column: x => x.PictureId,
                        principalTable: "ContentPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_ContentPage_ThumbImageId",
                        column: x => x.ThumbImageId,
                        principalTable: "ContentPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecAttr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    SpecId = table.Column<int>(nullable: false),
                    AttrValue = table.Column<string>(nullable: false),
                    OrjId = table.Column<int>(nullable: true),
                    LangId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecAttr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecAttr_Lang_LangId",
                        column: x => x.LangId,
                        principalTable: "Lang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecAttr_Spec_OrjId",
                        column: x => x.OrjId,
                        principalTable: "Spec",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecAttr_Spec_SpecId",
                        column: x => x.SpecId,
                        principalTable: "Spec",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecContentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    SpecId = table.Column<int>(nullable: false),
                    ContentTypesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecContentType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecContentType_ContentTypes_ContentTypesId",
                        column: x => x.ContentTypesId,
                        principalTable: "ContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecContentType_Spec_SpecId",
                        column: x => x.SpecId,
                        principalTable: "Spec",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecContentValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    SpecId = table.Column<int>(nullable: false),
                    ContentPageId = table.Column<int>(nullable: false),
                    ContentValue = table.Column<string>(nullable: true),
                    LangId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecContentValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecContentValue_ContentPage_ContentPageId",
                        column: x => x.ContentPageId,
                        principalTable: "ContentPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecContentValue_Lang_LangId",
                        column: x => x.LangId,
                        principalTable: "Lang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecContentValue_Spec_SpecId",
                        column: x => x.SpecId,
                        principalTable: "Spec",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceConfigAuth",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaDate = table.Column<DateTime>(nullable: false),
                    CreaUser = table.Column<int>(nullable: false),
                    ModUser = table.Column<int>(nullable: true),
                    ModDate = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<DateTime>(nullable: true),
                    IsStatus = table.Column<int>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true),
                    ServiceConfigId = table.Column<int>(nullable: false),
                    UsersId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    PermissionId = table.Column<int>(nullable: true),
                    IsCreate = table.Column<bool>(nullable: true),
                    IsRead = table.Column<bool>(nullable: true),
                    IsUpdate = table.Column<bool>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: true),
                    IsList = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceConfigAuth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceConfigAuth_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceConfigAuth_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceConfigAuth_ServiceConfig_ServiceConfigId",
                        column: x => x.ServiceConfigId,
                        principalTable: "ServiceConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceConfigAuth_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentPage_ContentPageId",
                table: "ContentPage",
                column: "ContentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPage_ContentTypesId",
                table: "ContentPage",
                column: "ContentTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPage_FormTypeId",
                table: "ContentPage",
                column: "FormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPage_LangId",
                table: "ContentPage",
                column: "LangId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPage_OrjId1",
                table: "ContentPage",
                column: "OrjId1");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_BannerImageId",
                table: "Documents",
                column: "BannerImageId",
                unique: true,
                filter: "[BannerImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentId",
                table: "Documents",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_GalleryId",
                table: "Documents",
                column: "GalleryId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PictureId",
                table: "Documents",
                column: "PictureId",
                unique: true,
                filter: "[PictureId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ThumbImageId",
                table: "Documents",
                column: "ThumbImageId",
                unique: true,
                filter: "[ThumbImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_FormTypeId",
                table: "Forms",
                column: "FormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_RoleId",
                table: "Permission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleParentId",
                table: "Role",
                column: "RoleParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ServiceConfigId",
                table: "Role",
                column: "ServiceConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceConfig_ParentId",
                table: "ServiceConfig",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceConfigAuth_PermissionId",
                table: "ServiceConfigAuth",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceConfigAuth_RoleId",
                table: "ServiceConfigAuth",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceConfigAuth_ServiceConfigId",
                table: "ServiceConfigAuth",
                column: "ServiceConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceConfigAuth_UsersId",
                table: "ServiceConfigAuth",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Spec_LangId",
                table: "Spec",
                column: "LangId");

            migrationBuilder.CreateIndex(
                name: "IX_Spec_OrjId",
                table: "Spec",
                column: "OrjId");

            migrationBuilder.CreateIndex(
                name: "IX_Spec_ParentId",
                table: "Spec",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecAttr_LangId",
                table: "SpecAttr",
                column: "LangId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecAttr_OrjId",
                table: "SpecAttr",
                column: "OrjId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecAttr_SpecId",
                table: "SpecAttr",
                column: "SpecId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecContentType_ContentTypesId",
                table: "SpecContentType",
                column: "ContentTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecContentType_SpecId",
                table: "SpecContentType",
                column: "SpecId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecContentValue_ContentPageId",
                table: "SpecContentValue",
                column: "ContentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecContentValue_LangId",
                table: "SpecContentValue",
                column: "LangId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecContentValue_SpecId",
                table: "SpecContentValue",
                column: "SpecId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "ServiceConfigAuth");

            migrationBuilder.DropTable(
                name: "SiteConfig");

            migrationBuilder.DropTable(
                name: "SpecAttr");

            migrationBuilder.DropTable(
                name: "SpecContentType");

            migrationBuilder.DropTable(
                name: "SpecContentValue");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "ContentPage");

            migrationBuilder.DropTable(
                name: "Spec");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "ContentTypes");

            migrationBuilder.DropTable(
                name: "FormType");

            migrationBuilder.DropTable(
                name: "Lang");

            migrationBuilder.DropTable(
                name: "ServiceConfig");
        }
    }
}
