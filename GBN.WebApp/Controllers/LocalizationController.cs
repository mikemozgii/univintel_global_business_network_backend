using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UnivIntel.GBN.Core.Models;

namespace UnivIntel.GBN.WebApp.Controllers
{
    [Route("api/1/localization")]
    [ApiController]
    public class LocalizationController : Controller
    {

        public static List<LocalizationItem> m_Localizations = new List<LocalizationItem>
        {
            new LocalizationItem
            {
                Key = "companies",
                English = "Companies",
                Russian = "Компании"
            },
            new LocalizationItem
            {
                Key = "editcompany",
                English = "Edit Company",
                Russian = "Изменить компанию"
            },
            new LocalizationItem
            {
                Key = "addcompany",
                English = "New Company",
                Russian = "Новая компания"
            },
            new LocalizationItem
            {
                Key = "companyname",
                English = "Company Name",
                Russian = "Имя компании"
            },
            new LocalizationItem
            {
                Key = "description",
                English = "Description",
                Russian = "Описание"
            },
            new LocalizationItem
            {
                Key = "founded",
                English = "Founded",
                Russian = "Основание"
            },
            new LocalizationItem
            {
                Key = "foundedismandatory",
                English = "Founded is mandatory",
                Russian = "Основание обязательное поле"
            },
            new LocalizationItem
            {
                Key = "nameismandatory",
                English = "Name is mandatory",
                Russian = "Имя компании обязательное поле"
            },
            new LocalizationItem
            {
                Key = "descriptionismandatory",
                English = "Description is mandatory",
                Russian = "Описание компании обязательное поле"
            },
            new LocalizationItem
            {
                Key = "tagline",
                English = "Tagline",
                Russian = "Теги"
            },
            new LocalizationItem
            {
                Key = "phone",
                English = "Phone",
                Russian = "Телефон"
            },
            new LocalizationItem
            {
                Key = "phoneismandatory",
                English = "Phone is mandatory",
                Russian = "Телефон обязательное поле"
            },
            new LocalizationItem
            {
                Key = "industry",
                English = "Industry",
                Russian = "Индустрия"
            },
            new LocalizationItem
            {
                Key = "companysize",
                English = "Company Size",
                Russian = "Размер компании"
            },
            new LocalizationItem
            {
                Key = "employees",
                English = "employees",
                Russian = "работников"
            },
            new LocalizationItem
            {
                Key = "save",
                English = "Save",
                Russian = "Сохранить"
            },
            new LocalizationItem
            {
                Key = "youdonothavecompanies",
                English = "You do not have companies",
                Russian = "У Вас еще нет компаний"
            },
            new LocalizationItem
            {
                Key = "dashboard",
                English = "Dashboard",
                Russian = "Дашборд"
            },
            new LocalizationItem
            {
                Key = "contacts",
                English = "Contacts",
                Russian = "Контакты"
            },
            new LocalizationItem
            {
                Key = "youdonothavecontacts",
                English = "Please add company contact information",
                Russian = "Пожалуйста добавьте контакты компании"
            },
            new LocalizationItem
            {
                Key = "addcontact",
                English = "New Contact",
                Russian = "Новый контакт"
            },
            new LocalizationItem
            {
                Key = "editcontact",
                English = "Edit Contact",
                Russian = "Изменить контакт"
            },
            new LocalizationItem
            {
                Key = "companyinfo",
                English = "Company Info",
                Russian = "О Компании"
            },
            new LocalizationItem
            {
                Key = "title",
                English = "Title",
                Russian = "Заголовок"
            },
            new LocalizationItem
            {
                Key = "fullname",
                English = "Full Name",
                Russian = "Полное имя"
            },
            new LocalizationItem
            {
                Key = "email",
                English = "E-mail",
                Russian = "E-mail"
            },
            new LocalizationItem
            {
                Key = "postalcode",
                English = "Postal code",
                Russian = "Почтовый индекс"
            },
            new LocalizationItem
            {
                Key = "addressline1",
                English = "Address line 1",
                Russian = "Адрес линия 1"
            },
            new LocalizationItem
            {
                Key = "addressline2",
                English = "Address line 2",
                Russian = "Адрес линия 2"
            },
            new LocalizationItem
            {
                Key = "city",
                English = "Location",
                Russian = "Местонахождение"
            },
            new LocalizationItem
            {
                Key = "requiredfield",
                English = "Field is required",
                Russian = "Обязательное поле"
            },
            new LocalizationItem
            {
                Key = "role",
                English = "Role",
                Russian = "Роль"
            },
            new LocalizationItem
            {
                Key = "news",
                English = "News & Blogs",
                Russian = "Новости и блоги"
            },
            new LocalizationItem
            {
                Key = "addnews",
                English = "Add News",
                Russian = "Добавить новость"
            },
            new LocalizationItem
            {
                Key = "youdonothavenews",
                English = "You do not have news",
                Russian = "У вас нет новостей"
            },
            new LocalizationItem
            {
                Key = "youdonothavelocations",
                English = "Please add location",
                Russian = "Пожалуйста, добавьте локации"
            },
            new LocalizationItem
            {
                Key = "locations",
                English = "Locations",
                Russian = "Локации"
            },
            new LocalizationItem
            {
                Key = "addlocation",
                English = "New Location",
                Russian = "Новая локация"
            },
            new LocalizationItem
            {
                Key = "editlocation",
                English = "Edit Location",
                Russian = "Изменить локацию"
            },
            new LocalizationItem
            {
                Key = "name",
                English = "Name",
                Russian = "Имя"
            },
            new LocalizationItem
            {
                Key = "investmentporfolio",
                English = "Investment Portfolio",
                Russian = "Финансовый портфель"
            },
            new LocalizationItem
            {
                Key = "companysummary",
                English = "Company Summary",
                Russian = "Профиль компании"
            },
            new LocalizationItem
            {
                Key = "managementteam",
                English = "Management Team",
                Russian = "Руководство компании",
            },
            new LocalizationItem
            {
                Key = "managementteamdescription",
                English = "Who are the members of your management team and how will their experience aid in your success?",
                Russian = ""
            },
            new LocalizationItem
            {
                Key = "customerproblem",
                English = "Customer Problem",
                Russian = "Решаемая проблема",
            },
            new LocalizationItem
            {
                Key = "customerproblemdescription",
                English = "What customer problem does your product and/or service solve?",
                Russian = "Какую проблему решает продукт или услуга"
            },
            new LocalizationItem
            {
                Key = "productsandservices",
                English = "Products & Services",
                Russian = "Продукты и услуги",
            },
            new LocalizationItem
            {
                Key = "productsandservicesdescription",
                English = "Describe the product or service that you will sell and how it solves the customer problem, listing the main value proposition for each product/service.",
                Russian = ""
            },
            new LocalizationItem
            {
                Key = "targetmarket",
                English = "Target Market",
                Russian = "Целевой рынок",
            },
            new LocalizationItem
            {
                Key = "targetmarketdescription",
                English = "Define the important geographic, demographic, and/or psychographic characteristics of the market within which your customer segments exist.",
                Russian = ""
            },
            new LocalizationItem
            {
                Key = "businessmodel",
                English = "Business Model",
                Russian = "Бизнес модель",
            },
            new LocalizationItem
            {
                Key = "businessmodeldescription",
                English = "What strategy will you employ to build, deliver, and retain company value (e.g., profits)?",
                Russian = ""
            },
            new LocalizationItem
            {
                Key = "customersegments",
                English = "Customer Segments",
                Russian = "Целевая аудитория",
            },
            new LocalizationItem
            {
                Key = "customersegmentsdescription",
                English = "Outline your targeted customer segments. These are the specific subsets of your target market that you will focus on to gain traction.",
                Russian = ""
            },
            new LocalizationItem
            {
                Key = "salesmarketingstrategy",
                English = "Sales & Marketing Strategy",
                Russian = "Стратегия маркетинга и продажи",
            },
            new LocalizationItem
            {
                Key = "salesmarketingstrategydescription",
                English = "What is your customer acquisition and retention strategy? Detail how you will promote, sell and create customer loyalty for your products and services.",
                Russian = ""
            },
            new LocalizationItem
            {
                Key = "сompetitors",
                English = "Competitors",
                Russian = "Конкуренты",
            },
            new LocalizationItem
            {
                Key = "сompetitorsdescription",
                English = "Describe the competitive landscape and your competitors’ strengths and weaknesses. If direct competitors don’t exist, describe the existing alternatives.",
                Russian = ""
            },
            new LocalizationItem
            {
                Key = "competitiveadvantage",
                English = "Competitive Advantage",
                Russian = "Конкурентное преимущество",
            },
            new LocalizationItem
            {
                Key = "competitiveadvantagedescription",
                English = "What is your company’s competitive or unfair advantage? This can include patents, first mover advantage, unique expertise, or proprietary processes/technology.",
                Russian = ""
            },
            new LocalizationItem
            {
                Key = "onelinepitch",
                English = "One Line Pitch",
                Russian = "Краткое описание"
            },
            new LocalizationItem
            {
                Key = "incorporationtype",
                English = "Incorporation Type",
                Russian = "Тип регистрации"
            },
            new LocalizationItem
            {
                Key = "companystage",
                English = "Company Stage",
                Russian = "Стадия"
            },
            new LocalizationItem
            {
                Key = "pitchvideolink",
                English = "Pitch video link",
                Russian = "Ссылка на видео презентацию"
            },
            new LocalizationItem
            {
                Key = "fieldnotfilled",
                English = "The field is not filled",
                Russian = "Поле не заполнено"
            },
            new LocalizationItem
            {
                Key = "incorparetedtypesnotincorparated",
                English = "Not Incorparated",
                Russian = "Not Incorparated"
            },
            new LocalizationItem
            {
                Key = "incorparetedtypesother",
                English = "Other",
                Russian = "Другое"
            },
            new LocalizationItem
            {
                Key = "incorparetedtypesc-corp",
                English = "C-corp",
                Russian = "C-corp"
            },
            new LocalizationItem
            {
                Key = "incorparetedtypess-corp",
                English = "S-corp",
                Russian = "S-corp"
            },
            new LocalizationItem
            {
                Key = "incorparetedtypesb-corp",
                English = "B-corp",
                Russian = "B-corp"
            },
            new LocalizationItem
            {
                Key = "incorparetedtypesllc",
                English = "LLC",
                Russian = "LLC"
            },
            new LocalizationItem
            {
                Key = "companystageconcept",
                English = "Concept Only",
                Russian = "Концепция"
            },
            new LocalizationItem
            {
                Key = "companystageindevelopment",
                English = "Product In Development",
                Russian = "Продукт в разработке"
            },
            new LocalizationItem
            {
                Key = "companystageprototype",
                English = "Prototype Ready",
                Russian = "Прототип готов"
            },
            new LocalizationItem
            {
                Key = "companystagefullproduct",
                English = "Full Product Ready",
                Russian = "Продукт польностью готов"
            },
            new LocalizationItem
            {
                Key = "companystage500k",
                English = "$500K in TTM Revenue",
                Russian = "$500K in TTM Revenue"
            },
            new LocalizationItem
            {
                Key = "companystage1m",
                English = "$1M in TTM Revenue",
                Russian = "$1M in TTM Revenue"
            },
            new LocalizationItem
            {
                Key = "companystage3m",
                English = "$3M in TTM Revenue",
                Russian = "$3M in TTM Revenue"
            },
            new LocalizationItem
            {
                Key = "companystage5m",
                English = "$5M in TTM Revenue",
                Russian = "$5M in TTM Revenue"
            },
            new LocalizationItem
            {
                Key = "companystage10m",
                English = "$10M in TTM Revenue",
                Russian = "$10M in TTM Revenue"
            },
            new LocalizationItem
            {
                Key = "companystage20m",
                English = "$20M in TTM Revenue",
                Russian = "$20M in TTM Revenue"
            },
            new LocalizationItem
            {
                Key = "companystage50m",
                English = "$50M in TTM Revenue",
                Russian = "$50M in TTM Revenue"
            },
            new LocalizationItem
            {
                Key = "companystagemore50m",
                English = "$50M+ in TTM Revenue",
                Russian = "$50M+ in TTM Revenue"
            },
            new LocalizationItem
            {
                Key = "annualfinancials",
                English = "Annual Financials",
                Russian = "Annual Financials"
            },
            new LocalizationItem
            {
                Key = "annualrevenuerunrate",
                English = "Annual Revenue Run Rate",
                Russian = "Annual Revenue Run Rate"
            },
            new LocalizationItem
            {
                Key = "monthlyburnrate",
                English = "Monthly Burn Rate",
                Russian = "Monthly Burn Rate"
            },
            new LocalizationItem
            {
                Key = "financialannotation",
                English = "Financial Annotation",
                Russian = "Финасовые заметки"
            },
            new LocalizationItem
            {
                Key = "revenuedriver",
                English = "Revenue Driver",
                Russian = "Revenue Driver"
            },
            new LocalizationItem
            {
                Key = "add",
                English = "Add",
                Russian = "Добавить"
            },
            new LocalizationItem
            {
                Key = "youdonothaveitems",
                English = "You Do not Have Items",
                Russian = "У вас нет позиций"
            },
            new LocalizationItem
            {
                Key = "year",
                English = "Year",
                Russian = "Год"
            },
            new LocalizationItem
            {
                Key = "revenue",
                English = "Revenue",
                Russian = "Доход"
            },
            new LocalizationItem
            {
                Key = "expenditure",
                English = "Expenditure",
                Russian = "Расход"
            },
            new LocalizationItem
            {
                Key = "yearincorrect",
                English = "Year incorrect",
                Russian = "Год не правильный"
            },
            new LocalizationItem
            {
                Key = "yearalreadyexists",
                English = "Year already exists",
                Russian = "Год уже добавлен"
            },
            new LocalizationItem
            {
                Key = "notuploaded",
                English = "Not uploaded yet",
                Russian = "Еще не загружен"
            },
            new LocalizationItem
            {
                Key = "upload",
                English = "Upload",
                Russian = "Загрузить"
            },
            new LocalizationItem
            {
                Key = "reupload",
                English = "Re-upload",
                Russian = "Обновить"
            },
            new LocalizationItem
            {
                Key = "businessplan",
                English = "Business Plan",
                Russian = "Бизнес план"
            },
            new LocalizationItem
            {
                Key = "financialprojection",
                English = "Financial Projections",
                Russian = "Финансовые прогнозы"
            },
            new LocalizationItem
            {
                Key = "supplementaldocument",
                English = "Supplemental Document",
                Russian = "Дополнительный документ"
            },
            new LocalizationItem
            {
                Key = "investor",
                English = "Investor",
                Russian = "Инвестор"
            },
            new LocalizationItem
            {
                Key = "capitalraised",
                English = "Capital Raised",
                Russian = "Поднято капитала"
            },
            new LocalizationItem
            {
                Key = "round",
                English = "Round",
                Russian = "Раунд"
            },
            new LocalizationItem
            {
                Key = "seeking",
                English = "Seeking",
                Russian = "Стремление"
            },
            new LocalizationItem
            {
                Key = "category",
                English = "Category",
                Russian = "Категория"
            },
            new LocalizationItem
            {
                Key = "fundinghistory",
                English = "Funding",
                Russian = "Финансирование"
            },
            new LocalizationItem
            {
                Key = "notspecified",
                English = "Not specified",
                Russian = "Не заполнено"
            },
            new LocalizationItem
            {
                Key = "onlynow",
                English = "Change the stage of financing",
                Russian = "Изменить этап финансирования"
            },
            new LocalizationItem
            {
                Key = "other",
                English = "Other",
                Russian = "Другое"
            },
            new LocalizationItem
            {
                Key = "roundother",
                English = "Other",
                Russian = "Другое"
            },
            new LocalizationItem
            {
                Key = "roundfounder",
                English = "Founder",
                Russian = "Основатель"
            },
            new LocalizationItem
            {
                Key = "roundfriends_and_family",
                English = "Friends and Family",
                Russian = "Друзья и семья"
            },
            new LocalizationItem
            {
                Key = "roundseries_seed",
                English = "Seed",
                Russian = "Семя"
            },
            new LocalizationItem
            {
                Key = "roundseries_a",
                English = "Series A",
                Russian = "Серия А"
            },
            new LocalizationItem
            {
                Key = "roundseries_b",
                English = "Series B",
                Russian = "Серия B"
            },
            new LocalizationItem
            {
                Key = "roundseries_c",
                English = "Series C",
                Russian = "Серия C"
            },
            new LocalizationItem
            {
                Key = "securitytypepreferred_equity",
                English = "Preferred Equity",
                Russian = "Предпочитаемые акции"
            },
            new LocalizationItem
            {
                Key = "securitytypecommon_equity",
                English = "Common Equity",
                Russian = "Обыкновенные акции"
            },
            new LocalizationItem
            {
                Key = "securitytypeconvertible_note",
                English = "Convertible Note",
                Russian = "Конвертируемая Банкнота"
            },
            new LocalizationItem
            {
                Key = "securitytype",
                English = "Security Type",
                Russian = "Тип безопастности"
            },
            new LocalizationItem
            {
                Key = "investorname",
                English = "Investor Name",
                Russian = "Инвестор"
            },
            new LocalizationItem
            {
                Key = "investoremail",
                English = "Investor Email",
                Russian = "Email инвестора"
            },
            new LocalizationItem
            {
                Key = "fundinground",
                English = "Funding Round",
                Russian = "Раунд инвестирования"
            },
            new LocalizationItem
            {
                Key = "pitchdeck",
                English = "Pitch Deck",
                Russian = "Файл презентации"
            },
            new LocalizationItem
            {
                Key = "couponsanddiscounts",
                English = "Coupons & Discounts",
                Russian = "Купоны и скидки"
            },
            new LocalizationItem
            {
                Key = "documents",
                English = "Documents",
                Russian = "Документы"
            },
            new LocalizationItem
            {
                Key = "changelanguage",
                English = "Language",
                Russian = "Язык"
            },
            new LocalizationItem
            {
                Key = "accountsettings",
                English = "Account Settings",
                Russian = "Настройки аккаунта"
            },
            new LocalizationItem
            {
                Key = "signout",
                English = "Sign out",
                Russian = "Выйти"
            },
            new LocalizationItem
            {
                Key = "urlisincorrect",
                English = "Url is incorrect",
                Russian = "Url некорректный"
            },
            new LocalizationItem
            {
                Key = "incorrect_data",
                English = "Incorrect Data",
                Russian = "Неверные параметры"
            },
            new LocalizationItem
            {
                Key = "abbreviation",
                English = "Abbreviation",
                Russian = "Аббревиатура"
            },
            new LocalizationItem
            {
                Key = "culture",
                English = "Culture",
                Russian = "Культура"
            },
            new LocalizationItem
            {
                Key = "cultureimage",
                English = "Culture image",
                Russian = "Культура на картинке"
            },
            new LocalizationItem
            {
                Key = "culturepresentation",
                English = "Video Url",
                Russian = "Видео Url"
            },
            new LocalizationItem
            {
                Key = "culturenotspecified",
                English = "Culture not specified",
                Russian = "Культура не указана"
            },
            new LocalizationItem
            {
                Key = "imagenotspecified",
                English = "Image not specified",
                Russian = "Изображение не указано"
            },
            new LocalizationItem
            {
                Key = "imageculturespecifyimage",
                English = "Specify Image",
                Russian = "Указать изображение"
            },
            new LocalizationItem
            {
                Key = "password",
                English = "Password",
                Russian = "Пароль"
            },
            new LocalizationItem
            {
                Key = "investmentsprofile",
                English = "Investments portfolio",
                Russian = "Портфолио инвестиций"
            },
            new LocalizationItem
            {
                Key = "mycompanies",
                English = "My Companies",
                Russian = "Мои компании"
            },
            new LocalizationItem
            {
                Key = "upc",
                English = "UPC",
                Russian = "Код"
            },
            new LocalizationItem
            {
                Key = "price",
                English = "Price",
                Russian = "Цена"
            },
            new LocalizationItem
            {
                Key = "link",
                English = "Link to External Resource",
                Russian = "Ссылка"
            },
            new LocalizationItem
            {
                Key = "donthaveanaccountyet",
                English = "Don't have an account yet?",
                Russian = "Нет учетной записи?"
            },
            new LocalizationItem
            {
                Key = "signup",
                English = "Sign Up",
                Russian = "Зарегистрироваться"
            },
            new LocalizationItem
            {
                Key = "login",
                English = "Sign In",
                Russian = "Войти"
            },
            new LocalizationItem
            {
                Key = "forgotpassword",
                English = "Forgot password?",
                Russian = "Забыли пароль"
            },
            new LocalizationItem
            {
                Key = "selectcity",
                English = "Enter City or Country",
                Russian = "Выберите город"
            },
            new LocalizationItem
            {
                Key = "firstname",
                English = "First Name",
                Russian = "Имя"
            },
            new LocalizationItem
            {
                Key = "lastname",
                English = "Last Name",
                Russian = "Фамилия"
            },
            new LocalizationItem
            {
                Key = "changepassword",
                English = "Change Password",
                Russian = "Сменить пароль"
            },
            new LocalizationItem
            {
                Key = "changelanguage",
                English = "Change Language",
                Russian = "Сменить Язык"
            },
            new LocalizationItem
            {
                Key = "onelinepitchempty",
                English = "Add a short presentation",
                Russian = "Добавьте краткую презентацию"
            },
            new LocalizationItem
            {
                Key = "pleaseselectanoption",
                English = "Please select an option",
                Russian = "Пожалуйста выбирете опцию"
            },
            new LocalizationItem
            {
                Key = "pitchvideolinkempty",
                English = "Upload a short video presentation video to make your company profile more impressive",
                Russian = "Загрузите короткий ролик с видео презентацией, чтобы профиль компании производил более сильное впечатление"
            },
            new LocalizationItem
            {
                Key = "newpassword",
                English = "New Password",
                Russian = "Новый Пароль"
            },
            new LocalizationItem
            {
                Key = "currentstageoffinancing",
                English = "Current stage of financing",
                Russian = "Текущий этап финансирования"
            },
            new LocalizationItem
            {
                Key = "previousfundingsteps",
                English = "Previous Funding Steps",
                Russian = "Предыдущие этапы финансирования"
            },
            new LocalizationItem
            {
                Key = "nocreatedorganizations",
                English = "No created organizations",
                Russian = "Нет созданных организаций"
            },
            new LocalizationItem
            {
                Key = "locationcategoryfood",
                English = "Food",
                Russian = "Поесть"
            },
            new LocalizationItem
            {
                Key = "locationcategoryauto",
                English = "Auto",
                Russian = "Авто"
            },
            new LocalizationItem
            {
                Key = "locationcategorybeauty",
                English = "Beauty",
                Russian = "Красота"
            },
            new LocalizationItem
            {
                Key = "locationcategoryhealth",
                English = "Health",
                Russian = "Здоровье"
            },
            new LocalizationItem
            {
                Key = "locationcategorygoods",
                English = "Goods",
                Russian = "Товары"
            },
            new LocalizationItem
            {
                Key = "locationcategoryservices",
                English = "Services",
                Russian = "Услуги"
            },
            new LocalizationItem
            {
                Key = "locationcategorytourism",
                English = "Tourism",
                Russian = "Туризм"
            },
            new LocalizationItem
            {
                Key = "locationcategoryproducts",
                English = "Products",
                Russian = "Продукты"
            },
            new LocalizationItem
            {
                Key = "locationcategorysport",
                English = "Sport",
                Russian = "Спорт"
            },
            new LocalizationItem
            {
                Key = "locationcategoryeducation",
                English = "Education",
                Russian = "Образование"
            },
            new LocalizationItem
            {
                Key = "locationcategorydevelopment",
                English = "Development",
                Russian = "Строительство"
            },
            new LocalizationItem
            {
                Key = "locationcategoryenterteinment",
                English = "Enterteinment",
                Russian = "Развлечение"
            },
            new LocalizationItem
            {
                Key = "yes",
                English = "Yes",
                Russian = "Да"
            },
            new LocalizationItem
            {
                Key = "no",
                English = "No",
                Russian = "Нет"
            },
            new LocalizationItem
            {
                Key = "companylimitconfirmmessage",
                English = "To register more than 1 company, you need to activate a premium subscription. Activate?",
                Russian = "Чтобы зарегистрировать более 1 компании, Вам необходимо активировать премиум-подписку. Активировать?"
            },
            new LocalizationItem
            {
                Key = "displayorganization",
                English = "Show to Public",
                Russian = "Показывать публично"
            },
            new LocalizationItem
            {
                Key = "companyemail",
                English = "Company email",
                Russian = "Email организации"
            },
            new LocalizationItem
            {
                Key = "code",
                English = "Code",
                Russian = "Код"
            },
            new LocalizationItem
            {
                Key = "displayoffer",
                English = "Display Offer?",
                Russian = "Показывать предложение"
            },
            new LocalizationItem
            {
                Key = "сreatediscount",
                English = "Create a Discount",
                Russian = "Создать Скидку"
            },
            new LocalizationItem
            {
                Key = "createcoupon",
                English = "Create a Coupon",
                Russian = "Создать Купон"
            },
            new LocalizationItem
            {
                Key = "delete",
                English = "Delete",
                Russian = "Удалить"
            },
            new LocalizationItem
            {
                Key = "displaycontact",
                English = "Show contact",
                Russian = "Показывать контакт"
            },
            new LocalizationItem
            {
                Key = "displaycontactemail",
                English = "Show email",
                Russian = "Показывать email контакта"
            },
            new LocalizationItem
            {
                Key = "canceldelete",
                English = "Cancel Delete?",
                Russian = "Отменить удаление?"
            },
            new LocalizationItem
            {
                Key = "promote",
                English = "Promote",
                Russian = "Продвинуть"
            },
            new LocalizationItem
            {
                Key = "displaylocation",
                English = "Show location",
                Russian = "Показывать место"
            },
            new LocalizationItem
            {
                Key = "monday",
                English = "Monday",
                Russian = "Понедельник"
            },
            new LocalizationItem
            {
                Key = "tuesday",
                English = "Tuesday",
                Russian = "Вторник"
            },
            new LocalizationItem
            {
                Key = "wednesday",
                English = "Wednesday",
                Russian = "Среда"
            },
            new LocalizationItem
            {
                Key = "thursday",
                English = "Thursday",
                Russian = "Четверг"
            },
            new LocalizationItem
            {
                Key = "friday",
                English = "Friday",
                Russian = "Пятница"
            },
            new LocalizationItem
            {
                Key = "saturday",
                English = "Saturday",
                Russian = "Суббота"
            },
            new LocalizationItem
            {
                Key = "sunday",
                English = "Sunday",
                Russian = "Воскресенье"
            },
            new LocalizationItem
            {
                Key = "product",
                English = "Product",
                Russian = "Товар"
            },
            new LocalizationItem
            {
                Key = "service",
                English = "Service",
                Russian = "Услгуа"
            },
            new LocalizationItem
            {
                Key = "createproduct",
                English = "Create a Product",
                Russian = "Добавить Товар"
            },
            new LocalizationItem
            {
                Key = "createservice",
                English = "Create a Service",
                Russian = "Добавить Услугу"
            },
            new LocalizationItem
            {
                Key = "discount",
                English = "Discount",
                Russian = "Скидка"
            },
            new LocalizationItem
            {
                Key = "coupon",
                English = "Coupon",
                Russian = "Купон"
            },
            new LocalizationItem
            {
                Key = "workinghours",
                English = "Manage Working Hours",
                Russian = "Управление рабочими часами"
            },
            new LocalizationItem
            {
                Key = "start",
                English = "Start",
                Russian = "Начало"
            },
            new LocalizationItem
            {
                Key = "end",
                English = "End",
                Russian = "Конец"
            },
            new LocalizationItem
            {
                Key = "displayworkinghourday",
                English = "Display working hours",
                Russian = "Отображать рабочие часы"
            },
            new LocalizationItem
            {
                Key = "hasdinner",
                English = "Lunch",
                Russian = "Обед"
            },
            new LocalizationItem
            {
                Key = "dinnerstart",
                English = "Start",
                Russian = "Начало обеда"
            },
            new LocalizationItem
            {
                Key = "dinnerend",
                English = "Dinner end",
                Russian = "Конец обеда"
            },
            new LocalizationItem
            {
                Key = "pleaseaddaproductorservice",
                English = "Please add a product or service",
                Russian = "Пожалуйста добвьте продукт или услугу"
            },
            new LocalizationItem
            {
                Key = "pleaseaddacouponordiscount",
                English = "Please add a coupon or discount",
                Russian = "Пожалуйста добавьте купон или дисконт"
            },
            new LocalizationItem
            {
                Key = "forcompaniesatrank",
                English = "For companies at Rank",
                Russian = "Для компаний с ранком"
            },
            new LocalizationItem
            {
                Key = "forselectedcompanies",
                English = "For Selected Companies",
                Russian = "Для выбранных компаний"
            },
            new LocalizationItem
            {
                Key = "availableatrank",
                English = "Available at rank",
                Russian = "Доступно с ранка"
            },
            new LocalizationItem
            {
                Key = "silver",
                English = "Silver",
                Russian = "Серебро"
            },
            new LocalizationItem
            {
                Key = "gold",
                English = "Gold",
                Russian = "Золото"
            },
            new LocalizationItem
            {
                Key = "newpromotion",
                English = "New Promotion",
                Russian = "Новое Продвижение"
            },
            new LocalizationItem
            {
                Key = "search",
                English = "Search",
                Russian = "Поиск"
            },
            new LocalizationItem
            {
                Key = "rank",
                English = "Rank",
                Russian = "Ранг"
            },
            new LocalizationItem
            {
                Key = "rankup",
                English = "Rank up",
                Russian = "Поднять"
            },
            new LocalizationItem
            {
                Key = "settings",
                English = "Settings",
                Russian = "Настройки"
            },
            new LocalizationItem
            {
                Key = "nocompaniesfound",
                English = "No companies found",
                Russian = "Компании не найдены"
            },
            new LocalizationItem
            {
                Key = "message",
                English = "Message",
                Russian = "Сообщение"
            },
            new LocalizationItem
            {
                Key = "displaynews",
                English = "Display news",
                Russian = "Показать новость"
            },
            new LocalizationItem
            {
                Key = "datestart",
                English = "Date Start",
                Russian = "Дата начала"
            },
            new LocalizationItem
            {
                Key = "dateend",
                English = "Date End",
                Russian = "Дата завершения"
            },
            new LocalizationItem
            {
                Key = "dateismandatory",
                English = "Date is Mandatory",
                Russian = "Дата обязательна"
            },
            new LocalizationItem
            {
                Key = "selectcompanies",
                English = "Select Companies",
                Russian = "Выбор компаний"
            },
            new LocalizationItem
            {
                Key = "promotes",
                English = "Promotes",
                Russian = "Продвижения"
            },
            new LocalizationItem
            {
                Key = "employeespage",
                English = "Employees",
                Russian = "Сотрудники"
            },
            new LocalizationItem
            {
                Key = "addemployee",
                English = "New Employees",
                Russian = "Новый сотрудник"
            },
            new LocalizationItem
            {
                Key = "from",
                English = "from",
                Russian = "из"
            },
            new LocalizationItem
            {
                Key = "editemployee",
                English = "Edit employee",
                Russian = "Редактировать сотрудника"
            },
            new LocalizationItem
            {
                Key = "confirmpassword",
                English = "Confirm Password",
                Russian = "Подтвердить пароль"
            },
            new LocalizationItem
            {
                Key = "companyinfooption",
                English = "Company Info",
                Russian = "Информация о компании"
            },
            new LocalizationItem
            {
                Key = "invitefriends",
                English = "Invite Friends",
                Russian = "Пригласить друзей"
            },
            new LocalizationItem
            {
                Key = "visible",
                English = "Visible",
                Russian = "Видимость"
            },
            new LocalizationItem
            {
                Key = "incorrectdate",
                English = "Incorrect Date",
                Russian = "Неверная дата"
            },
            new LocalizationItem
            {
                Key = "newproduct",
                English = "New Product",
                Russian = "Новый продукт"
            },
            new LocalizationItem
            {
                Key = "newservice",
                English = "New Service",
                Russian = "Новая услуга"
            },
            new LocalizationItem
            {
                Key = "newdiscount",
                English = "New Discount",
                Russian = "Новая скидка"
            },
            new LocalizationItem
            {
                Key = "newcoupon",
                English = "New Coupon",
                Russian = "Новый купон"
            },
            new LocalizationItem
            {
                Key = "forcomapnieswithminimumrankorhigher",
                English = "For Comapnies with minimum rank or higher",
                Russian = "Для компаний с минимальным ранком"
            },
            new LocalizationItem
            {
                Key = "additionalcompanies",
                English = "Additional Companies",
                Russian = "Доболнительные компании"
            },
            new LocalizationItem
            {
                Key = "free",
                English = "Free",
                Russian = "Бесплатно"
            },
            new LocalizationItem
            {
                Key = "currentprice",
                English = "Current Price",
                Russian = "Текущая цена"
            },
            new LocalizationItem
            {
                Key = "originalprice",
                English = "Original Price",
                Russian = "Цена"
            },
            new LocalizationItem
            {
                Key = "canreused",
                English = "Can Reused",
                Russian = "Повторное использование"
            },
            new LocalizationItem
            {
                Key = "upgradeaccount",
                English = "Upgrade Account",
                Russian = "Улучшить аккаунт"
            },
            new LocalizationItem
            {
                Key = "bio",
                English = "Bio",
                Russian = "Биография"
            },
            new LocalizationItem
            {
                Key = "workinghoursmap",
                English = "Working Hours",
                Russian = "Рабочие часы"
            },
            new LocalizationItem
            {
                Key = "biodescription",
                English = "Add a few words about yourself",
                Russian = "Добавьте пару слов о себе"
            },
            new LocalizationItem
            {
                Key = "account",
                English = "Account",
                Russian = "Аккаунт"
            },
            new LocalizationItem
            {
                Key = "usernamedescription",
                English = "Tap to change user name",
                Russian = "Нажмите для изменения имени"
            },
            new LocalizationItem
            {
                Key = "emaildescription",
                English = "Tap to change email",
                Russian = "Нажмите для изменения email"
            },
            new LocalizationItem
            {
                Key = "undo",
                English = "Undo",
                Russian = "Отменить"
            },
            new LocalizationItem
            {
                Key = "datepublish",
                English = "Date Publish",
                Russian = "Дата публикации"
            },
            new LocalizationItem
            {
                Key = "pleasecheckyouremailforsecuritycode",
                English = "Please check your email for Security Code",
                Russian = "Проверьте вашу почту для получения кода безопасности"
            },
            new LocalizationItem
            {
                Key = "securitycode",
                English = "Security Code",
                Russian = "Код Безопасности"
            },
            new LocalizationItem
            {
                Key = "contactphone",
                English = "Contact Phone",
                Russian = "Телефон"
            },
            new LocalizationItem
            {
                Key = "contactemail",
                English = "Contact Email",
                Russian = "Email"
            },
            new LocalizationItem
            {
                Key = "taptochange",
                English = "Tap to change",
                Russian = "Нажмите чтобы изменить"
            },
            new LocalizationItem
            {
                Key = "taptoupload",
                English = "Tap to upload",
                Russian = "Нажмите чтобы загрузить"
            },
            new LocalizationItem
            {
                Key = "displaycontactphone",
                English = "Show phone",
                Russian = "Показывать телефон"
            },
            new LocalizationItem
            {
                Key = "shownews",
                English = "Show news",
                Russian = "Показывать новость"
            },
            new LocalizationItem
            {
                Key = "downgrade",
                English = "Downgrade",
                Russian = "Отменить подписку"
            },
            new LocalizationItem
            {
                Key = "premium",
                English = "Premium",
                Russian = "Премиум"
            },
            new LocalizationItem
            {
                Key = "rounds",
                English = "Rounds",
                Russian = "Раунды"
            },
            new LocalizationItem
            {
                Key = "annualrevenuerunratedescription",
                English = "Forecasted revenue is the annual revenue you would receive if you extrapolated current revenue for the whole year.",
                Russian = "Прогнозная выручка, это годовая выручка, которую вы получили бы, если экстраполировать текущую выручку на весь год"
            },
            new LocalizationItem
            {
                Key = "monthlyburnratedescription",
                English = "Monthly average costs represent negative cash flow, expressing the amount of cash the company spends per month",
                Russian = "Среднемесячные затраты представляют собой отрицательный денежный поток, выражая объем денежных средств, которые компания тратит за месяц"
            },
            new LocalizationItem
            {
                Key = "revenuedriverdescription",
                English = "Source of profit, this is a key variable that affects your revenue (for example, sales revenue)",
                Russian = "Источник прибыли, это ключевая переменная, влияющая на вашу выручку (например выручку от продаж)"
            },
            new LocalizationItem
            {
                Key = "jobs",
                English = "Jobs",
                Russian = "Вакансии"
            },
            new LocalizationItem
            {
                Key = "fromheader",
                English = "From",
                Russian = "От"
            },
            new LocalizationItem
            {
                Key = "to",
                English = "To",
                Russian = "До"
            },
            new LocalizationItem
            {
                Key = "skills",
                English = "Skills",
                Russian = "Навыки"
            },
            new LocalizationItem
            {
                Key = "fulltime",
                English = "Full time",
                Russian = "Полный рабочий день"
            },
            new LocalizationItem
            {
                Key = "parttime",
                English = "Part time",
                Russian = "Частичная занятость"
            },
            new LocalizationItem
            {
                Key = "typeofposition",
                English = "Type of Position",
                Russian = "Вид занятости"
            },
            new LocalizationItem
            {
                Key = "workedexpirience",
                English = "Worked Experience",
                Russian = "Необходимый опыт"
            },
            new LocalizationItem
            {
                Key = "recruitingcontact",
                English = "Recruiting Contact",
                Russian = "Контакт"
            },
            new LocalizationItem
            {
                Key = "addjob",
                English = "New Job",
                Russian = "Новая вакансия"
            },
            new LocalizationItem
            {
                Key = "editjob",
                English = "Edit Job",
                Russian = "Изменение вакансии"
            },
            new LocalizationItem
            {
                Key = "showinvestmentportfoliopublic",
                English = "Show Investment Portfolio to Public",
                Russian = "Показывать инвестиционное портфолио"
            },
            new LocalizationItem
            {
                Key = "products",
                English = "Products",
                Russian = "Товары"
            },
            new LocalizationItem
            {
                Key = "discounts",
                English = "Discounts",
                Russian = "скидки"
            },
            new LocalizationItem
            {
                Key = "services",
                English = "Services",
                Russian = "Услуги"
            },
            new LocalizationItem
            {
                Key = "coupons",
                English = "Coupons",
                Russian = "Купоны"
            },
            new LocalizationItem
            {
                Key = "upgrade_to_premium",
                English = "Upgrade to Premium",
                Russian = "Улучшить до Премиум аккаунта"
            },
            new LocalizationItem
            {
                Key = "ranks",
                English = "Ranks",
                Russian = "Ранги"
            },
            //new_translation_position
        };

        [Route("all")]
        public IEnumerable<LocalizationItem> GetLocalizations() => m_Localizations.ToArray();

    }

}
