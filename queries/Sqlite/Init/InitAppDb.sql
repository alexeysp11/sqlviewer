-- Create tables 
 
CREATE TABLE IF NOT EXISTS settings
(
    settings_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    name VARCHAR(25) NOT NULL, 
    description VARCHAR(100), 
    value VARCHAR(100)
); 

CREATE TABLE IF NOT EXISTS dbg_log
(
    source_element VARCHAR(100), 
    log_timestamp DATETIME, 
    log_level VARCHAR(25), 
    msg TEXT, 
    add_description TEXT, 
    stack_trace TEXT
); 

CREATE TABLE IF NOT EXISTS translation 
(
    translation_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    context VARCHAR(75) NOT NULL, -- Name of UI element or module 
    english TEXT NOT NULL, 
    german TEXT, 
    russian TEXT, 
    spanish TEXT, 
    portuguese TEXT, 
    italian TEXT, 
    french TEXT, 
    ukranian TEXT, 
    dutch TEXT, 
    polish TEXT,
    czech TEXT, 
    serbian TEXT, 
    croatian TEXT, 
    swedish TEXT, 
    norwegian TEXT, 
    danish TEXT, 
    afrikaans TEXT,
    turkish TEXT,
    kazakh TEXT,
    armenian TEXT,
    georgian TEXT,
    romanian TEXT,
    bulgarian TEXT,
    albanian TEXT,
    greek TEXT, 
    indonesian TEXT, 
    malay TEXT, 
    korean TEXT, 
    japanese TEXT
); 

-- Insert data into tables 

INSERT INTO settings (name) SELECT 'language' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'language') = 0; 
INSERT INTO settings (name) SELECT 'auto_save' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'auto_save') = 0; 
INSERT INTO settings (name) SELECT 'font_size' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'font_size') = 0; 
INSERT INTO settings (name) SELECT 'font_family' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'font_family') = 0; 
INSERT INTO settings (name) SELECT 'tab_size' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'tab_size') = 0; 
INSERT INTO settings (name) SELECT 'word_wrap' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'word_wrap') = 0; 
INSERT INTO settings (name) SELECT 'default_rdbms' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'default_rdbms') = 0; 
INSERT INTO settings (name) SELECT 'active_rdbms' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'active_rdbms') = 0; 
INSERT INTO settings (name) SELECT 'server' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'server') = 0;
INSERT INTO settings (name) SELECT 'database' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'database') = 0;
INSERT INTO settings (name) SELECT 'port' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'port') = 0;
INSERT INTO settings (name) SELECT 'schema' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'schema') = 0; 
INSERT INTO settings (name) SELECT 'username' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'username') = 0; 
INSERT INTO settings (name) SELECT 'password' WHERE (SELECT COUNT(*) FROM settings WHERE name LIKE 'password') = 0; 

DELETE FROM settings WHERE name IN (SELECT name FROM settings GROUP BY name HAVING COUNT(*) > 1); 

INSERT INTO translation VALUES(1,'Login','Active RDBMS','Aktives RDBMS','Активная БД','RDBMS activo','RDBMS ativo','RDBMS attivo','SGBDR actif','Активна СУБД','Actieve RDBMS','Aktywny RDBMS','Aktivní RDBMS','Ацтиве РДБМС','Aktivni RDBMS','Aktiv RDBMS','Aktiv RDBMS','Aktiv RDBMS','Aktiewe RDBMS','Aktif RDBMS','Белсенді RDBMS','Ակտիվ RDBMS','აქტიური RDBMS','RDBMS activ','Активна RDBMS','RDBMS aktive','Ενεργό RDBMS','RDBMS Aktif','RDBMS aktif','활성 RDBMS','アクティブな RDBMS');
INSERT INTO translation VALUES(2,'Login','Database','Datenbank','База данных','Base de datos','Base de dados','Banca dati','Base de données','База даних','Database','Baza danych','Databáze','База података','Baza podataka','Databas','Database','Database','Databasis','Veri tabanı','Дерекқор','Տվյալների բազա','Მონაცემთა ბაზა','Bază de date','База данни','Baza e të dhënave','Βάση δεδομένων','Basis data','Pangkalan data','데이터 베이스','データベース');
INSERT INTO translation VALUES(3,'Login','Schema','Schema','Схема','Esquema','Esquema','Schema','Schéma','Схема','Schema','Schemat','Schéma','Шема','Shema','Schema','Skjema','Skema','Skema','Şema','Схема','Սխեման','სქემა','Schemă','Схема','Skema','Σχήμα','Skema','Skema','개요','スキーマ');
INSERT INTO translation VALUES(4,'Login','Username','Nutzername','Пользователь','Nombre de usuario','Nome do usuário','Nome utente','Nom d''utilisateur','Ім''я користувача','Gebruikersnaam','Nazwa użytkownika','Uživatelské jméno','Корисничко име','Korisničko ime','Användarnamn','Brukernavn','Brugernavn','Gebruikersnaam','Kullanıcı adı','Пайдаланушы аты','Օգտագործողի անունը','მომხმარებლის სახელი','Nume de utilizator','Потребителско име','Emri i përdoruesit','Όνομα χρήστη','Nama belakang','Nama pengguna','사용자 이름','ユーザー名');
INSERT INTO translation VALUES(5,'Login','Password','Passwort','Пароль','Contraseña','Senha','Parola d''ordine','Mot de passe','Пароль','Wachtwoord','Hasło','Heslo','парола','parola','Lösenord','Passord','Adgangskode','Wagwoord','Parola','Пароль','Գաղտնաբառ','პაროლი','Parola','Парола','Fjalëkalimi','Παρασύνθημα','Pasword','Kata laluan','비밀번호','パスワード');
INSERT INTO translation VALUES(6,'Login','Log In','Einloggen','Войти','Iniciar','Entrar','Accesso','Connexion','Увійти','Log in','Zaloguj sie','Přihlásit se','Пријавите се','Prijaviti se','Logga in','Logg Inn','Log på','Teken aan','Giriş yapmak','Кіру','Մուտք գործեք','Შესვლა','Autentificare','Вход','Identifikohu','Συνδεθείτε','Masuk','Log masuk','로그인','ログインする');
INSERT INTO translation VALUES(7,'Login','Cancel','Stornieren','Отмена','Cancelar','Cancelar','Annulla','Annuler','Скасувати','Annuleren','Anulować','Zrušení','Поништити','Otkazati','Avbryt','Avbryt','Afbestille','Kanselleer','İptal etmek','Болдырмау','Չեղարկել','გაუქმება','Anulare','Отказ','Anulo','Ματαίωση','Membatalkan','Batal','취소','キャンセル');
INSERT INTO translation VALUES(8,'Menu','File','Datei','Файл','Expediente','Arquivo','File','Fichier','Файл','Het dossier','Plik','Soubor','Датотека','Datoteka','Fil','Fil','Fil','lêer','Dosya','Файл','ֆայլ','ფაილი','Fişier','Файл','Skedari','αρχείο','Berkas','Fail','파일','ファイル');
INSERT INTO translation VALUES(9,'Menu','New','Schaffen','Создать','Crear','Crio','Creare','Créer','Створювати','Creëren','Stwórz','Vytvořit','Креирај','Stvoriti','Skapa','Skape','Skabe','Skep','Oluşturmak','жасау','ստեղծել','შექმნა','crea','създавам','krijojnë','δημιουργώ','membuat','cipta','창조하다','作成');
INSERT INTO translation VALUES(10,'Menu','Open','Offen','Открыть','Abrir','Abrir','Aprire','Ouvrir','Відчинено','Open','Otwarty','Otevřít','Отвори','Otvoren','Öppen','Åpen','Åben','Maak oop','Açık','Ашық','Բաց','ღია','Deschis','Отворете','Hapur','Ανοιξε','Membuka','Buka','열려 있는','開ける');
INSERT INTO translation VALUES(11,'Menu','Reopen','Wieder öffnen','Открыть снова','Reabrir','Reabrir','Riaprire','Rouvrir','Знову відкрити','Heropenen','Otworzyć na nowo','Znovu otevřít','Поново отворити','Ponovo otvoriti','Öppna igen','Åpne igjen','Åbn igen','Heropen','yeniden aç','Қайта ашыңыз','Վերաբացեք','ხელახლა გახსნა','Redeschide','Отвори отново','Rihap','Ξανανοίγω','Buka kembali','Buka semula','다시 열다','再開');
INSERT INTO translation VALUES(12,'Menu','Save','Speichern','Сохранить','Guardar','Salve','Salva','Enregistrer','Зберегти','Opslaan','Zapisać','Zachránit','сачувати','Uštedjeti','Spara','Lagre','Spare','Stoor','Kayıt etmek','Сақтау','Պահպանել','გადარჩენა','Salva','Запазване','Ruaj','Σώσει','Menghemat','Jimat','저장하다','セーブ');
INSERT INTO translation VALUES(13,'Menu','Save All','Alles speichern','Сохранить все','Guardar todos','Salve todas','Salva tutte','Enregistrer toutes','Зберегти всі','Sla alles op','Zapisz wszystko','Uložit vše','Сачувај све','Spremi sve','Spara allt','Lagre alt','Gem alle','Stoor alles','Hepsini kaydet','Барлығын сақтау','Պահպանել բոլորը','შეინახეთ ყველა','Salvează tot','Запази всичко','Ruaj te gjitha','Αποθήκευση όλων','Simpan semua','Simpan semua','모두 저장','すべてを救う');
INSERT INTO translation VALUES(14,'Menu','Open All','Alles öffnen','Открыть все','Abrir todos','Abrir todas','Aprire tutte','Ouvrir tous','Відчинено всі','Open alles','Otwórz wszystko','Otevřít vše','Отвори све','Otvori sve','Öppna alla','Åpne alle','Åbn alle','Maak Alles oop','Hepsini aç','Барлығын ашу','Բացեք բոլորը','გახსენი ყველა','Deschideți toate','Отворете всички','Hap të gjitha','Άνοιγμα όλων','Buka semua','Buka Semua','모두 열기','すべて開く');
INSERT INTO translation VALUES(15,'Menu','Close','Schließen','Закрыть','Cerrar','Feche','Chiudi','Fermer','Закрити','Sluiten','Zamknąć','Zavřít','Затворити','Zatvoriti','Att stänga','Å lukke','At lukke','Om toe te maak','Kapatmak','Жабу','Փակել','დაკეტვა','A inchide','Затварям','Per te mbyllur','Να κλείσω','Untuk menutup','Untuk Tutup','닫기','閉じる');
INSERT INTO translation VALUES(16,'Menu','Close All','Alles schließen','Закрыть все','Cerrar todos','Feche todas','Chiudi tutte','Fermer tous','Закрити всі','Sluit alles','Zamknij wszystko','Zavřít vše','Затворите све','Zatvori sve','Stäng alla','Lukk alle','Luk alle','Maak alles toe','Hepsini kapat','Барлығын жабу','Փակել բոլորը','Დახურე ყველა','Inchide tot','Затвори всички','Mbyll të gjitha','Κλείσιμο όλων','Tutup semua','Tutup semua','모두 닫기','すべて閉じる');
INSERT INTO translation VALUES(17,'Menu','Exit','Beenden','Выйти','Salir','Sair','Uscire','Quitter','Вийти','Verlaten','Wyjście','Výstup','Изађи','Izlaz','Utträde','Utgang','Afslut','Verlaat','Çıkmak','Шығу','Ելք','გასასვლელი','Ieșire','Изход','Dalje','Εξοδος','Keluar','Keluar','출구','出口');
INSERT INTO translation VALUES(18,'Menu','SQL file','SQL-Datei','SQL Файл','Archivo SQL','Arquivo SQL','File SQL','Fichier SQL','SQL Файл','SQL bestand','Plik SQL','SQL soubor','SQL фајл','SQL datoteka','SQL-fil','SQL-fil','SQL fil','SQL lêer','SQL dosyası','SQL файлы','SQL ֆայլ','SQL ფაილი','Fișier SQL','SQL файл','Skedar SQL','αρχείο SQL','Berkas SQL','Fail SQL','SQL 파일','SQL ファイル');
INSERT INTO translation VALUES(19,'Menu','Function','Funktion','Функция','Función','Função','Funzione','Fonction','Функція','Functie','Funkcja','Funkce','Функција','Funkcija','Funktion','Funksjon','Funktion','Funksie','Fonksiyon','Функция','Ֆունկցիա','ფუნქცია','Funcţie','Функция','Funksion','Συνάρτηση','Fungsi','Fungsi','기능','関数');
INSERT INTO translation VALUES(20,'Menu','Procedure','Prozedur','Процедура','Procedimiento','Procedimento','Procedura','Procédure','Процедури','Procedure','Procedura','Procedura','Процедура','Procedura','Procedur','Prosedyre','Procedure','Prosedure','Prosedür','Процедура','Ընթացակարգը','Პროცედურა','Procedură','Процедура','Procedura','Διαδικασία','Prosedur','Prosedur','수술','手順');
INSERT INTO translation VALUES(21,'Menu','Test','Prüfung','Тест','Prueba','Teste','Test','Test','Випробування','Test','Test','Test','Тест','Test','Testa','Test','Test','Toets','Test','Тест','Թեստ','ტესტი','Test','Тест','Test','Δοκιμή','Uji','Ujian','테스트','テスト');
INSERT INTO translation VALUES(22,'Menu','Database','Datenbank','База данных','Base de datos','Base de dados','Banca dati','Base de données','База даних','Database','Baza danych','Databáze','База података','Baza podataka','Databas','Database','Database','Databasis','Veri tabanı','Дерекқор','Տվյալների բազա','Მონაცემთა ბაზა','Bază de date','База данни','Baza e të dhënave','Βάση δεδομένων','Basis data','Pangkalan data','데이터 베이스','データベース');
INSERT INTO translation VALUES(23,'Menu','Table','Tabelle','Таблица','Tabla','Tabela','Tabella','Tableau','Таблиця','Tabel','Tabela','Tabulka','Табле','Tablica','Tabell','Tabell','Tabel','Tafel','Tablo','Кесте','Աղյուսակ','მაგიდა','Tabelul','Таблица','Tabela','Τραπέζι','Meja','Meja','테이블','テーブル');
INSERT INTO translation VALUES(24,'Menu','Sequence','Sequenz','Последовательность','Secuencia','Seqüência','Sequenza','Séquence','Послідовність','Sequentie','Sekwencja','Sekvence','Секвенца','Sekvencija','Sekvens','Sekvens','Sekvens','Volgorde','Sekans','Кезектілік','Հերթականություն','თანმიმდევრობა','Secvenţă','Последователност','Sekuenca','Αλληλουχία','Urutan','Urutan','반복 진행','シーケンス');
INSERT INTO translation VALUES(25,'Menu','View','Ansicht','Представление','Vista','Vista','Visualizzazione','Vue','Переглянути','Uitzicht','Pogląd','Pohled','Поглед','Pogled','Utsikt','Utsikt','Udsigt','Uitsig','Görüntüle','Көрініс','Դիտել','ხედი','Vedere','Изглед','Pamje','Θέα','Pandangan','Pandangan','견해','見晴らし');
INSERT INTO translation VALUES(26,'Menu','Trigger','Auslöser','Триггер','Desencadenar','Acionar','Grilletto','Gâchette','Тригер','Trigger','Spust','Spoušť','Триггер','Trigger','Trigger','Trigger','Udløser','Trigger','Tetiklemek','Триггер','ձգան','ტრიგერი','Trigger','Тригер','Shkrehës','σκανδάλη','Pemicu','Pencetus','방아쇠','引き金');
INSERT INTO translation VALUES(27,'Menu','Edit','Bearbeiten','Редактировать','Editar','Editar','Modificare','Éditer','Редагувати','Bewerking','Edytować','Upravit','Уредити','Uredi','Redigera','Redigere','Redigere','Redigeer','Düzenle','Түзету','Խմբագրել','რედაქტირება','Editare','Редактиране','Redakto','Επεξεργαστείτε','Sunting','Sunting','편집하다','編集');
INSERT INTO translation VALUES(28,'Menu','Undo','Rückgängig machen','Назад','Deshacer','Desfazer','Disfare','Annuler','Скасувати','Ongedaan maken','Cofnij','Zpět','Поништи','Poništi','Ångra','Angre','Fortryd','Maak ongedaan','Geri alma','Болдырмау','Հետարկել','გაუქმება','Anula','Отмяна','Zhbër','Αναίρεση','Urungkan','Buat asal','실행 취소','元に戻す');
INSERT INTO translation VALUES(29,'Menu','Redo','Wiederholen','Вперед','Rehacer','Refazer','Rifare','Refaire','Повторити','Opnieuw doen','Przerobić','Předělat','Понови','Ponovi','Göra om','Gjøre om','Gentag','Herhaal','Yeniden yap','Қайталау','Կրկնել','ხელახლა','Reface','Повторете','Ribëje','Ξανακάνω','Mengulangi','Buat semula','다시 하다','やり直し');
INSERT INTO translation VALUES(30,'Menu','Settings','Einstellungen','Настройки','Configuración','Configurações','Impostazioni','Paramètres','Параметри','Instellingen','Ustawienia','Nastavení','Подешавања','Postavke','Inställningar','Innstillinger','Indstillinger','Verstellings','Ayarlar','Параметрлер','Կարգավորումներ','პარამეტრები','Setări','Настройки','Cilësimet','Ρυθμίσεις','Setting','Tetapan','설정','設定');
INSERT INTO translation VALUES(31,'Menu','Pages','Seiten','Страницы','Paginas','Páginas','Pagine','Pages','Сторінки','Pagina''s','Strony','Stránky','Странице','Stranice','Sidor','Sider','Sider','Bladsye','Sayfalar','Беттер','Էջեր','გვერდები','Pagini','Страници','Faqet','Σελίδες','Halaman','Muka surat','페이지','ページ');
INSERT INTO translation VALUES(32,'Menu','SQL query','SQL-Abfrage','SQL запрос','Consulta SQL','Consulta SQL','Query SQL','Requête SQL','SQL-запит','SQL-query','Zapytanie SQL','SQL dotaz','SQL упит','SQL upit','SQL-fråga','SQL-spørring','SQL-forespørgsel','SQL-navraag','SQL sorgusu','SQL сұрауы','SQL հարցում','SQL შეკითხვა','Interogare SQL','SQL запитване','Kërkesa SQL','Ερώτημα SQL','Permintaan SQL','Pertanyaan SQL','SQL 쿼리','SQL クエリ');
INSERT INTO translation VALUES(33,'Menu','Command line','Befehlszeile','Коммандная строка','Línea de comando','Linha de comando','Linea di comando','Ligne de commande','Командний рядок','Opdrachtregel','Wiersz poleceń','Příkazový řádek','Командна линија','Naredbeni redak','Kommandorad','Kommandolinje','Kommandolinje','Bevelreël','Komut satırı','Пәрмен жолы','Հրամանի տող','ბრძანების ხაზი','Linie de comanda','Командна линия','Linja e komandës','Γραμμή εντολών','Garis komando','Barisan arahan','명령줄','コマンドライン');
INSERT INTO translation VALUES(34,'Menu','Functions','Funktionen','Функции','Funciones','Funções','Funzioni','Les fonctions','Функції','Functies','Funkcje','Funkce','Функције','Funkcije','Funktioner','Funksjoner','Funktioner','Funksies','Fonksiyonlar','Функциялар','Ֆունկցիաներ','ფუნქციები','Funcții','Функции','Funksione','Λειτουργίες','Fungsi','Fungsi','함수들','機能');
INSERT INTO translation VALUES(35,'Menu','Procedures','Verfahren','Процедуры','Procedimientos','Procedimentos','Procedure','Procédures','Процедури','Procedures','Procedury','Procedury','Процедуре','Procedure','Förfaranden','Prosedyrer','Procedurer','Prosedures','Prosedürler','Процедуралар','Ընթացակարգերը','პროცედურები','Proceduri','Процедури','Procedurat','Διαδικασίες','Prosedur','Prosedur','수속','手続き');
INSERT INTO translation VALUES(36,'Menu','Tests','Tests','Тесты','Pruebas','Testes','Test','Essais','Тести','Testen','Testy','Testy','Тестови','Testovi','Tester','Tester','Tests','Toetse','Testler','Тесттер','Թեստեր','ტესტები','Teste','Тестове','Testet','Δοκιμές','Tes','Ujian','테스트','テスト');
INSERT INTO translation VALUES(37,'Menu','Databases','Datenbanken','Базы данных','Bases de Datos','Bancos de dados','Banche dati','Bases de données','Бази даних','Databases','Bazy danych','Databáze','Базе података','Baze podataka','Databaser','Databaser','Databaser','Databasisse','Veritabanları','Мәліметтер базалары','Տվյալների բազաներ','მონაცემთა ბაზები','Baze de date','Бази данни','Bazat e të dhënave','Βάσεις δεδομένων','Database','Pangkalan data','데이터베이스','データベース');
INSERT INTO translation VALUES(38,'Menu','Tables','Tabellen','Таблицы','Tablas','Tabelas','Tabelle','Les tables','Таблиці','Tabellen','Tabele','Tabulky','Табеле','Tablicama','Tabeller','Tabeller','Tabellerne','Tabellen','Tabloları','Кестелер','սեղաններ','მაგიდები','Tabele','Таблици','Tabelat','τραπέζια','Tabel','Meja','테이블','テーブル');
INSERT INTO translation VALUES(39,'Menu','Tools','Werkzeuge','Инструменты','Herramientas','Ferramentas','Strumenti','Outils','Інструменти','Gereedschap','Narzędzia','Nástroje','Алати','Alati','Verktyg','Verktøy','Værktøjer','Gereedskap','Araç gereç','Құралдар','Գործիքներ','ხელსაწყოები','Instrumente','Инструменти','Mjetet','Εργαλεία','Peralatan','Alatan','툴','ツール');
INSERT INTO translation VALUES(40,'Menu','Options','Optionen','Опции','Opciones','Opções','Opzioni','Choix','Параметри','Opties','Opcje','Možnosti','Опције','Opcije','Alternativ','Alternativer','Valgmuligheder','Opsies','Seçenekler','Опциялар','Ընտրանքներ','ოფციები','Opțiuni','Опции','Opsione','Επιλογές','Opsi','Opsyen','선택사항','オプション');
INSERT INTO translation VALUES(41,'Menu','Toolbars','Werkzeugkästen','Панель инструментов','Barras de herramientas','Barras de ferramentas','Barre degli strumenti','Barres d''outils','Панелі інструментів','Werkbalken','Paski narzędzi','Panely nástrojů','Траке са алаткама','Alatne trake','Verktygsfält','Verktøylinjer','Værktøjslinjer','Werkbalke','Araç Çubukları','Құралдар тақталары','Գործիքադարակներ','ხელსაწყოების ზოლები','Bare de instrumente','Ленти с инструменти','Shiritat e veglave','Γραμμές εργαλείων','Toolbar','Bar alat','도구 모음','ツールバー');
INSERT INTO translation VALUES(42,'Menu','Connections','Anschlüsse','Соединения','Conexiones','Conexões','Connessioni','Connexions','Підключення','Verbindingen','Znajomości','Spojení','Везе','Veze','Anslutningar','Tilkoblinger','Forbindelser','Verbindings','Bağlantılar','Қосылымдар','Միացումներ','კავშირები','Conexiuni','Връзки','Lidhjet','Συνδέσεις','Koneksi','Perhubungan','연결','接続');
INSERT INTO translation VALUES(43,'Menu','Help','Hilfe','Помощь','Ayuda','Ajuda','Aiuto','Aider','Допомога','Helpen','Pomoc','Pomoc','Помоћ','Pomoć','Hjälp','Hjelp','Hjælp','Help','Yardım','Көмек','Օգնություն','დახმარება','Ajutor','Помогне','Ndihmë','Αρωγή','Membantu','Tolong','돕다','ヘルプ');
INSERT INTO translation VALUES(44,'Menu','Common SQL docs','Allgemeine SQL-Dokumentation','Общие документы по SQL','Documentos comunes de SQL','Documentos SQL comuns','Documenti SQL comuni','Documentation SQL commune','Загальні документи SQL','Algemene SQL-documenten','Wspólne dokumenty SQL','Běžné dokumenty SQL','Уобичајени SQL документи','Uobičajeni SQL dokumenti','Vanliga SQL-dokument','Vanlige SQL-dokumenter','Almindelige SQL-dokumenter','Algemene SQL-dokumente','Ortak SQL belgeleri','Жалпы SQL құжаттары','Ընդհանուր SQL փաստաթղթեր','საერთო SQL დოკუმენტები','Documente SQL comune','Общи SQL документи','Dokumentet e zakonshme SQL','Συνήθη έγγραφα SQL','Dokumen SQL umum','Dokumen SQL biasa','공통 SQL 문서','一般的な SQL ドキュメント');
INSERT INTO translation VALUES(45,'Menu','User guide','Benutzerhandbuch','Руководство пользователю','Guía del usuario','Guia de usuario','Guida utente','Manuel de l''utilisateur','Посібник користувача','Gebruikershandleiding','Podręcznik użytkownika','Uživatelská příručka','Упутство за употребу','Upute za upotrebu','Användarguide','Brukerhåndboken','Brugervejledning','Gebruikersgids','Kullanici rehberi','Пайдаланушы нұсқаулығы','Օգտագործողի ուղեցույց','Მომხმარებლის სახელმძღვანელო','Manualul utilizatorului','Ръководство за потребителя','Udhëzues Përdorues','Οδηγός χρήσης','Panduan pengguna','Panduan pengguna','사용자 설명서','ユーザーガイド');
INSERT INTO translation VALUES(46,'Menu','GitHub repository','GitHub-Repository','GitHub репозиторий','Repositorio de GitHub','Repositório do GitHub','Repository GitHub','Référentiel GitHub','Репозиторій GitHub','GitHub-opslagplaats','Repozytorium GitHub','úložiště GitHub','GitHub репозиторијум','Repozitorij GitHub','GitHub-förråd','GitHub repository','GitHub repository','GitHub-bewaarplek','GitHub deposu','GitHub репозиторийі','GitHub պահոց','GitHub საცავი','Depozitul GitHub','GitHub хранилище','Depoja e GitHub','Αποθετήριο GitHub','Repositori GitHub','Repositori GitHub','GitHub 저장소','GitHub リポジトリ');
INSERT INTO translation VALUES(47,'Menu','Report','Prüfbericht','Отчет об ошибке','Informe','Relatório','Rapporto','Reportage','Звіт','Rapport','Raport','Zpráva','Извештај','Izvješće','Rapportera','Rapportere','Rapport','Rapporteer','Bildirmek','Есеп беру','Հաշվետվություն','მოხსენება','Raport','Доклад','Raportoni','αναφορά','Laporan','Laporan','보고서','報告する');
INSERT INTO translation VALUES(48,'Settings','Editor','Editor','Редактор','Editor','Editor','Editore','Éditeur','Редактор','Editor','Redaktor','Editor','Уредник','Urednik','Redaktör','Redaktør','Redaktør','Redakteur','Editör','Редактор','Խմբագիր','რედაქტორი','Editor','Редактор','Redaktor','Editor','Editor','Editor','편집자','編集者');
INSERT INTO translation VALUES(49,'Settings','Language','Sprache','Язык','Lenguaje','Linguagem','Lingua','Langue','Мову','Taal','Język','Jazyk','Језик','Jezik','Språk','Språk','Sprog','Taal','Dil','Тіл','Լեզու','Ენა','Limbă','Език','Gjuhë','Γλώσσα','Bahasa','Bahasa','언어','言語');
INSERT INTO translation VALUES(50,'Settings','Auto save','Automatisch speichern','Автосохранение','Guardado automático','Salvamento automático','Salvataggio automatico','Sauvegarde automatique','Автозбереження','Automatisch opslaan','Automatyczne zapisywanie','Automatické ukládání','Аутоматско спремање','Automatsko spremanje','Automatisk sparning','Autolagring','Automatisk lagring','Outo stoor','Otomatik kaydet','Автоматты сақтау','Ավտոմատ պահպանում','ავტომატური შენახვა','Salvare automata','Автоматично запазване','Ruajtja automatike','Αυτόματη αποθήκευση','Penyimpanan otomatis','Simpanan automatik','자동 저장','自動保存');
INSERT INTO translation VALUES(51,'Settings','Font size','Schriftgröße','Размер шрифта','Tamaño de fuente','Tamanho da fonte','Dimensione del font','Taille de police','Розмір шрифту','Lettertypegrootte','Rozmiar czcionki','Velikost fontu','Величина фонта','Veličina fonta','Fontstorlek','Skriftstørrelse','Skriftstørrelse','Skrifgrootte','Yazı Boyutu','Шрифт өлшемі','Տառատեսակի չափը','Შრიფტის ზომა','Mărimea fontului','Размер на шрифта','Përmasa e germave','Μέγεθος γραμματοσειράς','Ukuran huruf','Saiz huruf','글꼴 크기','フォントサイズ');
INSERT INTO translation VALUES(52,'Settings','Font family','Schriftfamilie','Шрифт','Familia tipográfica','Família de fontes','Famiglia di font','Famille de polices','Сімейство шрифтів','Lettertypefamilie','Rodzina czcionek','Rodina fontů','Породица фонтова','Porodica fontova','Typsnittsfamilj','Fontfamilie','Fontfamilie','Font familie','Font ailesi','Шрифттер тобы','Տառատեսակի ընտանիք','შრიფტის ოჯახი','Familie de fonturi','Шрифтово семейство','Familja e shkronjave','Οικογένεια γραμματοσειρών','Keluarga fonta','Keluarga font','글꼴 패밀리','フォントファミリー');
INSERT INTO translation VALUES(53,'Settings','Tab size','Tab-Größe','Размер табуляции','Tamaño de pestaña','Tamanho da guia','Dimensione della scheda','Taille de l''onglet','Розмір вкладки','Tabbladgrootte','Rozmiar zakładki','Velikost tabulátoru','Величина картице','Veličina kartice','Flikstorlek','Fanestørrelse','Fanestørrelse','Tabgrootte','Sekme boyutu','Қойынды өлшемі','Ներդիրի չափը','ჩანართის ზომა','Dimensiunea filei','Размер на табулатора','Madhësia e skedës','Μέγεθος καρτέλας','Ukuran tab','Saiz tab','탭 크기','タブサイズ');
INSERT INTO translation VALUES(54,'Settings','Word wrap','Zeilenumbruch','Перенос слов','Ajuste de línea','Quebra de linha','Involucro di parole','Retour à la ligne','Перенос слів','Woordterugloop','Zawijanie tekstu','Obtékání slov','Прелом редова','Prelom riječi','Ordlindning','Ordbryting','Tekstombrydning','Woordomhulsel','Kelime sarma','Сөз тасымалы','Բառի փաթաթում','სიტყვების შეფუთვა','Înveliș cu cuvinte','Обвиване с думи','Mbështjellja e fjalëve','Περιτύλιγμα λέξεων','Bungkus kata','Bungkus perkataan','자동 줄 바꿈','ワードラップ');
INSERT INTO translation VALUES(55,'Settings','DB','DB','БД','Base de datos','BD','DB','BD','БД','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB','DB');
INSERT INTO translation VALUES(56,'Settings','Default RDBMS','Standard-RDBMS','БД по умолчанию','RDBMS predeterminado','RDBMS padrão','RDBMS predefinito','SGBDR par défaut','СУБД за замовчуванням','Standaard RDBMS','Domyślny RDBMS','Standardní RDBMS','Подразумевани RDBMS','Default RDBMS','Standard RDBMS','Standard RDBMS','Standard RDBMS','Standaard-RDBMS','Varsayılan RDBMS','Әдепкі RDBMS','Կանխադրված RDBMS','სტანდარტული RDBMS','RDBMS implicit','Стандартна RDBMS','RDBMS e parazgjedhur','Προεπιλεγμένο RDBMS','RDBMS default','RDBMS lalai','기본 RDBMS','デフォルトの RDBMS');
INSERT INTO translation VALUES(57,'Settings','Active RDBMS','Aktives RDBMS','Активная БД','RDBMS activo','RDBMS ativo','RDBMS attivo','SGBDR actif','Активна СУБД','Actieve RDBMS','Aktywny RDBMS','Aktivní RDBMS','Ацтиве РДБМС','Aktivni RDBMS','Aktiv RDBMS','Aktiv RDBMS','Aktiv RDBMS','Aktiewe RDBMS','Aktif RDBMS','Белсенді RDBMS','Ակտիվ RDBMS','აქტიური RDBMS','RDBMS activ','Активна RDBMS','RDBMS aktive','Ενεργό RDBMS','RDBMS Aktif','RDBMS aktif','활성 RDBMS','アクティブな RDBMS');
INSERT INTO translation VALUES(58,'Settings','Database','Datenbank','База данных','Base de datos','Base de dados','Banca dati','Base de données','База даних','Database','Baza danych','Databáze','База података','Baza podataka','Databas','Database','Database','Databasis','Veri tabanı','Дерекқор','Տվյալների բազա','Მონაცემთა ბაზა','Bază de date','База данни','Baza e të dhënave','Βάση δεδομένων','Basis data','Pangkalan data','데이터 베이스','データベース');
INSERT INTO translation VALUES(59,'Settings','Schema','Schema','Схема','Esquema','Esquema','Schema','Schéma','Схема','Schema','Schemat','Schéma','Шема','Shema','Schema','Skjema','Skema','Skema','Şema','Схема','Սխեման','სქემა','Schemă','Схема','Skema','Σχήμα','Skema','Skema','개요','スキーマ');
INSERT INTO translation VALUES(60,'Settings','Username','Nutzername','Пользователь','Nombre de usuario','Nome do usuário','Nome utente','Nom d''utilisateur','Ім''я користувача','Gebruikersnaam','Nazwa użytkownika','Uživatelské jméno','Корисничко име','Korisničko ime','Användarnamn','Brukernavn','Brugernavn','Gebruikersnaam','Kullanıcı adı','Пайдаланушы аты','Օգտագործողի անունը','მომხმარებლის სახელი','Nume de utilizator','Потребителско име','Emri i përdoruesit','Όνομα χρήστη','Nama belakang','Nama pengguna','사용자 이름','ユーザー名');
INSERT INTO translation VALUES(61,'Settings','Password','Passwort','Пароль','Contraseña','Senha','Parola d''ordine','Mot de passe','Пароль','Wachtwoord','Hasło','Heslo','парола','parola','Lösenord','Passord','Adgangskode','Wagwoord','Parola','Пароль','Գաղտնաբառ','პაროლი','Parola','Парола','Fjalëkalimi','Παρασύνθημα','pasword','Kata laluan','비밀번호','パスワード');
INSERT INTO translation VALUES(62,'Settings','Recover','Wiederherstellung','Восстановить','Recuperar','Recuperar','Recuperare','Récupérer','Відновити','Herstellen','Odzyskiwać','Obnovit se','Опоравити се','Oporaviti se','återfå','Komme seg','Gendanne','Herstel','Kurtarmak','Қалпына келтіріңіз','Վերականգնել','აღდგენა','Recupera','Възстанови се','Rikuperoni','Αναρρώνω','Memulihkan','Pulih','복구','回復');
INSERT INTO translation VALUES(63,'Settings','Save','Speichern','Сохранить','Salvar','Salvar','Salva','Sauver','Зберегти','Opslaan','Zapisać','zachránit','Сачувати','Uštedjeti','Spara','Lagre','Spare','Stoor','Kayıt etmek','Сақтау','Պահպանել','გადარჩენა','Salva','Запазване','Ruaj','Σώσει','Menghemat','Jimat','저장하다','セーブ');
INSERT INTO translation VALUES(64,'Settings','Cancel','Stornieren','Отмена','Cancelar','Cancelar','Annulla','Annuler','Скасувати','Annuleren','Anulować','Zrušení','Поништити','Otkazati','Avbryt','Avbryt','Afbestille','Kanselleer','İptal etmek','Болдырмау','Չեղարկել','გაუქმება','Anulare','Отказ','Anulo','Ματαίωση','Membatalkan','Batal','취소','キャンセル');
INSERT INTO translation VALUES(65,'Common','Enabled','Ermöglicht','Включено','Activado','Habilitado','Abilitato','Autorisé','Увімкнено','Ingeschakeld','Włączony','Zapnuto','Омогућено','Omogućeno','Aktiverad','Aktivert','Aktiveret','Geaktiveer','Etkinleştirilmiş','Іске қосылған','Միացված է','გააქტიურებულია','Activat','Активирано','Aktivizuar','Ενεργοποιημένο','Diaktifkan','Didayakan','활성화됨','有効');
INSERT INTO translation VALUES(66,'Common','Disabled','Deaktiviert','Отключено','Discapacitado','Desabilitado','Disabilitato','Désactivé','Вимкнено','Gehandicapt','Wyłączony','Vypnuto','Онемогућено','Onemogućeno','Inaktiverad','Deaktivert','deaktiveret','Gedeaktiveer','Engelli','Өшірілген','Հաշմանդամ','ინვალიდი','Dezactivat','Дезактивиран','I paaftë','Ανάπηρος','Dengan disabilitas','Dilumpuhkan','비활성화된','無効');
INSERT INTO translation VALUES(67,'Pages','Path','Pfad','Путь','Sendero','Caminho','Sentiero','Chemin','Шлях','Pad','Ścieżka','Cesta','Пут','Staza','Väg','Sti','Sti','Pad','Yol','Жол','Ուղին','ბილიკი','Calea','Пътека','Rrugë','Μονοπάτι','Jalur','Laluan','오솔길','パス');
INSERT INTO translation VALUES(68,'Pages','Execute','Ausführen','Выполнить','Ejecutar','Executar','Eseguire','Exécuter','Виконати','Uitvoeren','Wykonać','Vykonat','Извршити','Izvršiti','Avrätta','Utføre','Udfør','Uit te voer','Yürüt','Орындау','Կատարել','შეასრულეთ','A executa','Изпълни','Ekzekutoni','Εκτελώ','Menjalankan','Laksanakan','실행하다','実行する');
INSERT INTO translation VALUES(69,'Pages','Tables','Tabellen','Таблицы','Tablas','Tabelas','Tabelle','les tables','Таблиці','Tabellen','Tabele','Tabulky','Табеле','Tablicama','Tabeller','Tabeller','Tabellerne','Tabellen','tabloları','кестелер','սեղաններ','მაგიდები','Tabele','Таблици','tabelat','τραπέζια','tabel','meja','테이블','テーブル');
INSERT INTO translation VALUES(70,'Pages','General info','Allgemeine Information','Общие данные','Información general','Informações gerais','Informazioni generali','Informations générales','Загальна інформація','Algemene informatie','Ogólne informacje','Obecné informace','Опште информације','Opće informacije','Generell information','Generell info','Generel info','Algemene info','Genel Bilgi','Жалпы ақпарат','Ընդհանուր տեղեկություններ','Ზოგადი ინფორმაცია','Informatii generale','Обща информация','Informacion i përgjithshëm','Γενικές πληροφορίες','Informasi Umum','Maklumat umum','일반 정보','一般的な情報');
INSERT INTO translation VALUES(71,'Pages','Columns','Säulen','Столбцы','Columnas','Colunas','Colonne','Colonnes','Колонки','Kolommen','Kolumny','Sloupce','Колумне','Kolone','Kolumner','Kolonner','Kolonner','Kolomme','Kolonlar','Бағаналар','Սյունակներ','Სვეტები','Coloane','Колони','Kolonat','Στηλών','Kolom','Lajur','칼럼','コラム');
INSERT INTO translation VALUES(72,'Pages','Foreign keys','Fremde Schlüssel','Внешние ключи','Llaves extranjeras','Chaves estrangeiras','Chiavi esterne','Clés étrangères','Сторонні ключі','Buitenlandse sleutels','Klucz obcy','Cizí klíče','Страни кључеви','Strani ključevi','Främmande nycklar','Fremmednøkler','Fremmednøgler','Buitelandse sleutels','Yabancı anahtarlar','Шетелдік кілттер','Արտասահմանյան բանալիներ','უცხოური გასაღებები','Chei străine','Външни ключове','Çelësat e huaj','Ξένα κλειδιά','Kunci asing','Kunci asing','외래 키','外部キー');
INSERT INTO translation VALUES(73,'Pages','Triggers','Trigger','Триггеры','Disparadores','Desencadeia','Trigger','Déclencheurs','Тригери','Triggers','Wyzwalacze','Triggers','Triggers','Triggers','Triggers','Triggere','Triggers','Triggers','Tetikleyiciler','Триггерлер','Գործարկիչներ','ტრიგერები','Declanșatoare','Тригери','Triggers','Πυροδοτήσεις','Triggers','Triggers','트리거','トリガー');
INSERT INTO translation VALUES(74,'Pages','Data','Daten','Данные','Datos','Dados','Dati','Données','Дані','Gegevens','Dane','Údaje','Подаци','Podaci','Data','Data','Data','Data','Veri','Деректер','Տվյալներ','მონაცემები','Date','Данни','Të dhënat','Δεδομένα','Data','Data','데이터','データ');
INSERT INTO translation VALUES(75,'Connection','Active RDBMS','Aktives RDBMS','Активная БД','RDBMS activo','RDBMS ativo','RDBMS attivo','SGBDR actif','Активна СУБД','Actieve RDBMS','Aktywny RDBMS','Aktivní RDBMS','Ацтиве РДБМС','Aktivni RDBMS','Aktiv RDBMS','Aktiv RDBMS','Aktiv RDBMS','Aktiewe RDBMS','Aktif RDBMS','Белсенді RDBMS','Ակտիվ RDBMS','აქტიური RDBMS','RDBMS activ','Активна RDBMS','RDBMS aktive','Ενεργό RDBMS','RDBMS Aktif','RDBMS aktif','활성 RDBMS','アクティブな RDBMS');
INSERT INTO translation VALUES(76,'Connection','Execute','Ausführen','Выполнить','Ejecutar','Executar','Eseguire','Exécuter','Виконати','Uitvoeren','Wykonać','Vykonat','Извршити','Izvršiti','Avrätta','Utføre','Udfør','Uit te voer','Yürüt','Орындау','Կատարել','შეასრულეთ','A executa','Изпълни','Ekzekutoni','Εκτελώ','Menjalankan','Laksanakan','실행하다','実行する');
INSERT INTO translation VALUES(77,'Connection','Transfer','Übertragen','Перенести','Transferir','Transferir','Trasferire','Transférer','Трансфер','Overdragen','Przenosić','Převod','Трансфер','Transfer','Överföra','Overføre','Overførsel','Oordrag','Aktar','Тасымалдау','Փոխանցում','ტრანსფერი','Transfer','Трансфер','Transferimi','Μεταβίβαση','Memindahkan','Pemindahan','옮기다','移行');
INSERT INTO translation VALUES(78,'Login','Port','Port','Порт','Puerto','Porta','Porto','Port','Порт','Poort','Port','Port','Порт','Port','Port','Port','Port','Poort','Bağlantı Noktası','Порт','Պորտ','პორტი','Port','Порт','Porti','Λιμάνι','Pelabuhan','Pelabuhan','포트','ポート');
INSERT INTO translation VALUES(79,'Settings','Port','Port','Порт','Puerto','Porta','Porto','Port','Порт','Poort','Port','Port','Порт','Port','Port','Port','Port','Poort','Bağlantı Noktası','Порт','Պորտ','პორტი','Port','Порт','Porti','Λιμάνι','Pelabuhan','Pelabuhan','포트','ポート');