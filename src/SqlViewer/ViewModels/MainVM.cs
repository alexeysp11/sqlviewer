using System.Data;
using System.Windows;
using System.Windows.Input;
using SqlViewer.Utils.Language;
using SqlViewer.Helpers;
using SqlViewer.ViewModels.Commands;

namespace SqlViewer.ViewModels;

public class MainVM
{
    public MainWindow MainWindow { get; private set; }

    public DataVM DataVM { get; private set; }
    public VisualVM VisualVM { get; private set; }

    public ConfigHelper ConfigHelper { get; private set; }

    public ICommand DbCommand { get; private set; }
    public ICommand HelpCommand { get; private set; }
    public ICommand RedirectCommand { get; private set; }
    public ICommand AppCommand { get; private set; }

    public Translator Translator { get; private set; }

    public MainVM(MainWindow mainWindow)
    {
        MainWindow = mainWindow;

        DataVM = new DataVM(this);
        VisualVM = new VisualVM(this);

        ConfigHelper = new ConfigHelper(this, SettingsHelper.RootFolder);

        DbCommand = new DbCommand(this);
        HelpCommand = new HelpCommand();
        RedirectCommand = new RedirectCommand(this);
        AppCommand = new AppCommand(this);

        (Translator = new Translator()).SetAppDbConnection((Models.DbConnections.SqliteDbConnection)DataVM.AppRdbmsPreproc.GetAppDbConnection());
    }

    /// <summary>
    /// Initializes AppRepository and UserDbConnection after getting settings from DB 
    /// </summary>
    public void InitAppRepository()
    {
        try
        {
            DataTable dt = DataVM.SendSqlRequest(@"
SELECT
    t.*,
    CASE WHEN language IN ('Arabic', 'Persian', 'Hebrew', 'Yidish') THEN 1 ELSE 0 END AS f_right_to_left
FROM (
    SELECT
        CASE
            WHEN UPPER(tmp.language) LIKE '%ENGISH%' THEN 'English'
            WHEN UPPER(tmp.language) LIKE '%GERMAN%' THEN 'German'
            WHEN UPPER(tmp.language) LIKE '%RUSSIAN%' THEN 'Russian'
            WHEN UPPER(tmp.language) LIKE '%SPANISH%' THEN 'Spanish'
            WHEN UPPER(tmp.language) LIKE '%PORTUGUESE%' THEN 'Portuguese'
            WHEN UPPER(tmp.language) LIKE '%ITALIAN%' THEN 'Italian'
            WHEN UPPER(tmp.language) LIKE '%FRENCH%' THEN 'French'
            WHEN UPPER(tmp.language) LIKE '%UKRANIAN%' THEN 'Ukranian'
            WHEN UPPER(tmp.language) LIKE '%DUTCH%' THEN 'Dutch'
            WHEN UPPER(tmp.language) LIKE '%POLISH%' THEN 'Polish'
            WHEN UPPER(tmp.language) LIKE '%CZECH%' THEN 'Czech'
            WHEN UPPER(tmp.language) LIKE '%SERBIAN%' THEN 'Serbian'
            WHEN UPPER(tmp.language) LIKE '%CROATIAN%' THEN 'Croatian'
            WHEN UPPER(tmp.language) LIKE '%SWEDISH%' THEN 'Swedish'
            WHEN UPPER(tmp.language) LIKE '%NORWEGIAN%' THEN 'Norwegian'
            WHEN UPPER(tmp.language) LIKE '%DANISH%' THEN 'Danish'
            WHEN UPPER(tmp.language) LIKE '%AFRIKAANS%' THEN 'Afrikaans'
            WHEN UPPER(tmp.language) LIKE '%TURKISH%' THEN 'Turkish'
            WHEN UPPER(tmp.language) LIKE '%KAZAKH%' THEN 'Kazakh'
            WHEN UPPER(tmp.language) LIKE '%ARMENIAN%' THEN 'Armenian'
            WHEN UPPER(tmp.language) LIKE '%GEORGIAN%' THEN 'Georgian'
            WHEN UPPER(tmp.language) LIKE '%ROMANIAN%' THEN 'Romanian'
            WHEN UPPER(tmp.language) LIKE '%BULGARIAN%' THEN 'Bulgarian'
            WHEN UPPER(tmp.language) LIKE '%ALBANIAN%' THEN 'Albanian'
            WHEN UPPER(tmp.language) LIKE '%GREEK%' THEN 'Greek'
            WHEN UPPER(tmp.language) LIKE '%INDONESIAN%' THEN 'Indonesian'
            WHEN UPPER(tmp.language) LIKE '%MALAY%' THEN 'Malay'
            WHEN UPPER(tmp.language) LIKE '%KOREAN%' THEN 'Korean'
            WHEN UPPER(tmp.language) LIKE '%JAPANESE%' THEN 'Japanese'
            ELSE 'English'
        END AS language,
        CASE
            WHEN UPPER(tmp.auto_save) LIKE '%ENABLED%' OR UPPER(tmp.auto_save) LIKE '1' THEN 'Enabled'
            ELSE 'Disabled'
        END AS auto_save,
        CASE
            WHEN CAST(tmp.font_size AS INTEGER) IN (8,9,10,11,12,14,16,18,20) THEN CAST(tmp.font_size AS INTEGER)
            ELSE 8
        END AS font_size,
        CASE
            WHEN UPPER(tmp.font_family) IN ('CONSOLAS') THEN tmp.font_family
            ELSE 'Consolas'
        END AS font_family,
        CASE
            WHEN CAST(tmp.tab_size AS INTEGER) BETWEEN 1 AND 8 THEN CAST(tmp.tab_size AS INTEGER)
            ELSE 4
        END AS tab_size,
        CASE
            WHEN UPPER(tmp.word_wrap) LIKE '%ENABLED%' OR UPPER(tmp.word_wrap) LIKE '1' THEN 'Enabled'
            ELSE 'Disabled'
        END AS word_wrap,
        CASE
            WHEN UPPER(tmp.default_rdbms) IN ('SQLITE', 'POSTGRESQL', 'MYSQL', 'ORACLE') THEN tmp.default_rdbms
            ELSE 'SQLite'
        END AS default_rdbms,
        CASE
            WHEN UPPER(tmp.active_rdbms) IN ('SQLITE', 'POSTGRESQL', 'MYSQL', 'ORACLE') THEN tmp.active_rdbms
            ELSE 'SQLite'
        END AS active_rdbms,
        CAST(tmp.server AS TEXT) AS server,
        CAST(tmp.db_name AS TEXT) AS db_name,
        CAST(tmp.port AS TEXT) AS port,
        CAST(tmp.schema_name AS TEXT) AS schema_name,
        CAST(tmp.db_username AS TEXT) AS db_username,
        CAST(tmp.db_pswd AS TEXT) AS db_pswd
    FROM (
        SELECT
            MAX(tt.language) AS language,
            MAX(tt.auto_save) AS auto_save,
            MAX(tt.font_size) AS font_size,
            MAX(tt.font_family) AS font_family,
            MAX(tt.tab_size) AS tab_size,
            MAX(tt.word_wrap) AS word_wrap,
            MAX(tt.default_rdbms) AS default_rdbms,
            MAX(tt.active_rdbms) AS active_rdbms,
            MAX(tt.server) AS server,
            MAX(tt.db_name) AS db_name,
            MAX(tt.port) AS port,
            MAX(tt.schema_name) AS schema_name,
            MAX(tt.db_username) AS db_username,
            MAX(tt.db_pswd) AS db_pswd
        FROM (
            SELECT
                CASE 
                    WHEN UPPER(t.name) LIKE '%LANG%' OR UPPER(t.name) LIKE '%LANGUAGE%' THEN COALESCE(t.value, 'English')
                    ELSE ''
                END AS language,
                CASE 
                    WHEN UPPER(t.name) LIKE '%AUTOSAVE%' OR UPPER(t.name) LIKE '%AUTO_SAVE%' THEN COALESCE(t.value, 'Enabled')
                    ELSE ''
                END AS auto_save,
                CASE 
                    WHEN UPPER(t.name) LIKE '%FONT_SIZE%' THEN COALESCE(t.value, '8')
                    ELSE ''
                END AS font_size,
                CASE 
                    WHEN UPPER(t.name) LIKE '%FONT_FAMILY%' THEN COALESCE(t.value, 'Consolas')
                    ELSE ''
                END AS font_family,
                CASE 
                    WHEN UPPER(t.name) LIKE '%TAB_SIZE%' THEN COALESCE(t.value, '8')
                    ELSE ''
                END AS tab_size,
                CASE 
                    WHEN UPPER(t.name) LIKE '%WORD_WRAP%' THEN COALESCE(t.value, 'Enabled')
                    ELSE ''
                END AS word_wrap,
                CASE
                    WHEN UPPER(t.name) LIKE '%DEFAULT_RDBMS%' THEN COALESCE(t.value, 'SQLite')
                    ELSE ''
                END AS default_rdbms,
                CASE 
                    WHEN UPPER(t.name) LIKE '%ACTIVE_RDBMS%' THEN COALESCE(t.value, 'SQLite')
                    ELSE ''
                END AS active_rdbms,
                CASE 
                    WHEN UPPER(t.name) LIKE '%SERVER%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS server,
                CASE 
                    WHEN UPPER(t.name) LIKE '%DATABASE%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS db_name,
                CASE 
                    WHEN UPPER(t.name) LIKE '%PORT%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS port,
                CASE 
                    WHEN UPPER(t.name) LIKE '%SCHEMA%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS schema_name,
                CASE 
                    WHEN UPPER(t.name) LIKE '%USERNAME%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS db_username,
                CASE 
                    WHEN UPPER(t.name) LIKE '%PASSWORD%' OR UPPER(t.name) LIKE '%PSWD%' THEN COALESCE(t.value, '')
                    ELSE ''
                END AS db_pswd
            FROM (
                SELECT DISTINCT s.name, s.value
                FROM settings s
                WHERE s.name IS NOT NULL
            ) t
        ) tt
    ) tmp
    LIMIT 1
) t;
");

            string language = dt.Rows[0]["language"].ToString();
            string autoSave = dt.Rows[0]["auto_save"].ToString();
            int fontSize = System.Convert.ToInt32(dt.Rows[0]["font_size"]);
            string fontFamily = dt.Rows[0]["font_family"].ToString();
            int tabSize = System.Convert.ToInt32(dt.Rows[0]["tab_size"]);
            string wordWrap = dt.Rows[0]["word_wrap"].ToString();
            string defaultRdbms = dt.Rows[0]["default_rdbms"].ToString();
            string activeRdbms = dt.Rows[0]["active_rdbms"].ToString();
            string server = dt.Rows[0]["server"].ToString();
            string dbName = dt.Rows[0]["db_name"].ToString();
            string port = dt.Rows[0]["port"].ToString();
            string schemaName = dt.Rows[0]["schema_name"].ToString();
            string dbUsername = dt.Rows[0]["db_username"].ToString();
            string dbPswd = dt.Rows[0]["db_pswd"].ToString();

            Models.EnumOperations.EnumEncoder enumEncoder = EnumCodecHelper.EnumEncoder;
            RepoHelper.SetAppSettingsRepo(new Models.DataStorage.AppSettingsRepo(enumEncoder, language, autoSave,
                fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms, server,
                dbName, port, schemaName, dbUsername, dbPswd));
            DataVM.InitUserDbConnection();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Translates all pages in the application 
    /// </summary>
    public void Translate()
    {
        try
        {
            Translator.SetLanguageEnum(RepoHelper.AppSettingsRepo.Language);
            Translator.TranslateLogin();
            Translator.TranslateMenu();
            Translator.TranslateSettings();
            Translator.TranslatePages();
            Translator.TranslateConnection();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void RecoverSettings()
    {
        string msg = "Are you sure to recover settings changes?";
        if (MessageBox.Show(msg, "Recover settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            DataVM.ClearTempTable("settings");

            string sql = @"
UPDATE settings SET value = 'English' WHERE name LIKE 'language';
UPDATE settings SET value = 'Enabled' WHERE name LIKE 'auto_save';
UPDATE settings SET value = '8' WHERE name LIKE 'font_size';
UPDATE settings SET value = 'Consolas' WHERE name LIKE 'font_family';
UPDATE settings SET value = '4' WHERE name LIKE 'tab_size';
UPDATE settings SET value = '8' WHERE name LIKE 'word_wrap';
UPDATE settings SET value = 'SQLite' WHERE name LIKE 'default_rdbms';
UPDATE settings SET value = 'SQLite' WHERE name LIKE 'active_rdbms';
UPDATE settings SET value = '' WHERE name LIKE 'database';
UPDATE settings SET value = '' WHERE name LIKE 'schema';
UPDATE settings SET value = '' WHERE name LIKE 'username';
UPDATE settings SET value = '' WHERE name LIKE 'password';";
            DataVM.SendSqlRequest(sql);
            InitAppRepository();
            Translate();
            VisualVM.InitUI();

            MessageBox.Show("Settings recovered", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public void SaveSettings()
    {
        string msg = "Are you sure to save settings changes?";
        if (MessageBox.Show(msg, "Save settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            try
            {
                ((Views.SettingsView)VisualVM.SettingsView).UpdateAppRepository();

                string sql = @"
UPDATE settings SET value = '{0}' WHERE UPPER(name) LIKE 'LANGUAGE';
UPDATE settings SET value = '{1}' WHERE UPPER(name) LIKE 'AUTO_SAVE';
UPDATE settings SET value = '{2}' WHERE UPPER(name) LIKE 'FONT_SIZE';
UPDATE settings SET value = '{3}' WHERE UPPER(name) LIKE 'FONT_FAMILY';
UPDATE settings SET value = '{4}' WHERE UPPER(name) LIKE 'TAB_SIZE';
UPDATE settings SET value = '{5}' WHERE UPPER(name) LIKE 'WORD_WRAP';";
                sql = string.Format(
                    sql,
                    RepoHelper.AppSettingsRepo.Language,
                    RepoHelper.AppSettingsRepo.AutoSave,
                    Models.EnumOperations.EnumDecoder.GetFontSizeName(RepoHelper.AppSettingsRepo.FontSize),
                    RepoHelper.AppSettingsRepo.FontFamily,
                    Models.EnumOperations.EnumDecoder.GetTabSizeName(RepoHelper.AppSettingsRepo.TabSize),
                    RepoHelper.AppSettingsRepo.WordWrap);
                DataVM.SendSqlRequest(sql);

                sql = @"
UPDATE settings SET value = '{0}' WHERE UPPER(name) LIKE 'DEFAULT_RDBMS';
UPDATE settings SET value = '{1}' WHERE UPPER(name) LIKE 'ACTIVE_RDBMS';
UPDATE settings SET value = '{2}' WHERE UPPER(name) LIKE 'SERVER';
UPDATE settings SET value = '{3}' WHERE UPPER(name) LIKE 'DATABASE';
UPDATE settings SET value = '{4}' WHERE UPPER(name) LIKE 'PORT';
UPDATE settings SET value = '{5}' WHERE UPPER(name) LIKE 'SCHEMA';
UPDATE settings SET value = '{6}' WHERE UPPER(name) LIKE 'USERNAME';
UPDATE settings SET value = '{7}' WHERE UPPER(name) LIKE 'PASSWORD';";
                sql = string.Format(
                    sql,
                    RepoHelper.AppSettingsRepo.DefaultRdbms,
                    RepoHelper.AppSettingsRepo.ActiveRdbms,
                    RepoHelper.AppSettingsRepo.DbHost,
                    RepoHelper.AppSettingsRepo.DbName,
                    RepoHelper.AppSettingsRepo.DbPort,
                    RepoHelper.AppSettingsRepo.DbSchema,
                    RepoHelper.AppSettingsRepo.DbUsername,
                    RepoHelper.AppSettingsRepo.DbPassword);
                DataVM.SendSqlRequest(sql);

                InitAppRepository();
                Translate();
                VisualVM.InitUI();
                MessageBox.Show("Settings saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                VisualVM.SettingsView.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public void CancelSettings()
    {
        string msg = "Are you sure to cancel settings changes?";
        if (MessageBox.Show(msg, "Cancel settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            DataVM.ClearTempTable("settings");

            ((Views.SettingsView)VisualVM.SettingsView).CancelChangesAppRepository();

            MessageBox.Show("Settings cancelled", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            VisualVM.SettingsView.Close();
        }
    }

    public static void ExitApplication()
    {
        string msg = "Are you sure to close the application?";
        if (MessageBox.Show(msg, "Exit the application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            Application.Current.Shutdown();
        }
    }
}
