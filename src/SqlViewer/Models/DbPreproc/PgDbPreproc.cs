using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewer.Models.DbConnections; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    public class PgDbPreproc : IDbPreproc
    {
        private MainVM MainVM { get; set; }

        public ICommonDbConnectionSV AppDbConnection { get; private set; }
        public ICommonDbConnectionSV UserDbConnection { get; private set; }

        public PgDbPreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            this.AppDbConnection = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db"); 
        }

        public void CreateDb()
        {
            System.Windows.MessageBox.Show("Postgres CreateDb"); 
        }
        public void OpenDb()
        {
            System.Windows.MessageBox.Show("Postgres OpenDb"); 
        } 

        public void InitUserDbConnection()
        {
            if (RepoHelper.AppSettingsRepo == null)
                throw new System.Exception("RepoHelper.AppSettingsRepo is not assigned."); 
            if (RepoHelper.AppSettingsRepo.ActiveRdbms != RdbmsEnum.PostgreSQL)
                throw new System.Exception($"Unable to initialize UserDbConnection, incorrect ActiveRdbms: '{RepoHelper.AppSettingsRepo.ActiveRdbms}'.");
            
            if (RepoHelper.AppSettingsRepo != null)
                UserDbConnection = new PgDbConnection();
        }

        public void DisplayTablesInDb()
        {
            if (UserDbConnection == null)
            {
                return; 
            }

            string sqlRequest = @"SELECT t.schemaname || '.' || t.relname AS name FROM (SELECT schemaname, relname FROM pg_stat_user_tables) t"; 
            DataTable dt = UserDbConnection.ExecuteSqlCommand(sqlRequest);
            MainVM.MainWindow.TablesPage.tvTables.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                TreeViewItem item = new TreeViewItem(); 
                item.Header = row["name"].ToString();
                MainVM.MainWindow.TablesPage.tvTables.Items.Add(item); 
            }
            MainVM.MainWindow.TablesPage.tvTables.IsEnabled = true; 
            MainVM.MainWindow.TablesPage.tvTables.Visibility = Visibility.Visible;
        }

        public void GetAllDataFromTable(string tableName)
        {
            string sqlRequest = $"SELECT * FROM {tableName}"; 
            MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        }

        public void GetColumnsOfTable(string tableName)
        {
            string[] tn = tableName.Split('.');
            string sqlRequest = string.Format(@"
SELECT
column_name,
ordinal_position,
column_default,
is_nullable,
data_type,
is_self_referencing,
is_generated,
is_updatable
FROM information_schema.columns
WHERE table_schema LIKE '{0}' AND table_name LIKE '{1}'", tn[0], tn[1]); 
            MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        }

        public void GetForeignKeys(string tableName)
        {
            string sqlRequest = string.Format(@"
SELECT
tc.table_schema,
tc.constraint_name,
tc.table_name,
kcu.column_name,
ccu.table_schema AS foreign_table_schema,
ccu.table_name AS foreign_table_name,
ccu.column_name AS foreign_column_name
FROM 
information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
ON tc.constraint_name = kcu.constraint_name
AND tc.table_schema = kcu.table_schema
JOIN information_schema.constraint_column_usage AS ccu
ON ccu.constraint_name = tc.constraint_name
AND ccu.table_schema = tc.table_schema
WHERE tc.constraint_type = 'FOREIGN KEY' AND tc.table_name LIKE '{0}';", tableName);
            MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        }

        public void GetTriggers(string tableName)
        {
            string sqlRequest = string.Format(@"SELECT * FROM information_schema.triggers WHERE event_object_table LIKE '{0}'", tableName);
            MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        }

        public void GetSqlDefinition(string tableName)
        {
            string[] tn = tableName.Split('.');
            string sqlRequest = string.Format(@"
CREATE OR REPLACE FUNCTION fGetSqlFromTable(aSchemaName VARCHAR(255), aTableName VARCHAR(255))
RETURNS TEXT
LANGUAGE plpgsql AS
$func$
DECLARE
i INTEGER;
lNumRec INTEGER;
rec RECORD;
lResult text;
BEGIN
i := 0;
SELECT COUNT(*) INTO lNumRec FROM information_schema.columns WHERE table_schema LIKE aSchemaName AND table_name LIKE aTableName;
lResult := 'CREATE TABLE ' || aSchemaName || '.' || aTableName || chr(10) || '(' || chr(10);
FOR rec IN (
    SELECT 
        column_name, 
        column_default, 
        is_nullable, 
        data_type, 
        character_maximum_length
    FROM information_schema.columns
    WHERE table_schema LIKE aSchemaName AND table_name LIKE aTableName
)
LOOP
    i := i + 1;
    lResult := lResult || '    ' || rec.column_name || ' ' || rec.data_type;
    IF UPPER(rec.data_type) LIKE '%CHAR%VAR%' THEN 
        lResult := lResult || '(' || rec.character_maximum_length || ')';
    END IF;
    IF rec.column_default IS NOT NULL AND rec.column_default <> '' THEN
        lResult := lResult || ' DEFAULT ' || rec.column_default;
    END IF;
    IF i = lNumRec THEN 
        lResult := lResult || chr(10) || ');';
    ELSE 
        lResult := lResult || ', ' || chr(10);
    END IF;
END LOOP;

RETURN lResult;
END
$func$;

SELECT fGetSqlFromTable('{0}', '{1}') AS sql;", tn[0], tn[1]);
            DataTable dt = UserDbConnection.ExecuteSqlCommand(sqlRequest);
            if (dt.Rows.Count > 0) 
            {
                DataRow row = dt.Rows[0];
                MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = row["sql"].ToString();
            }
            else 
            {
                MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = string.Empty;
            }
        } 

        public void SendSqlRequest()
        {
            if (UserDbConnection == null)
                throw new System.Exception("Database is not opened.");

            DataTable resultCollection = UserDbConnection.ExecuteSqlCommand(MainVM.MainWindow.SqlPage.mtbSqlRequest.Text);
            MainVM.MainWindow.SqlPage.dbgSqlResult.ItemsSource = resultCollection.DefaultView;

            MainVM.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible; 
            MainVM.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true;
        }

        public DataTable SendSqlRequest(string sql)
        {
            if (AppDbConnection == null)
                throw new System.Exception("System RDBMS is not assigned."); 
            return AppDbConnection.ExecuteSqlCommand(sql);
        }

        public void ClearTempTable(string tableName)
        {

        } 

        public ICommonDbConnectionSV GetAppDbConnection()
        {
            return AppDbConnection; 
        }

        public ICommonDbConnectionSV GetUserDbConnection()
        {
            return UserDbConnection; 
        }
    }
}
