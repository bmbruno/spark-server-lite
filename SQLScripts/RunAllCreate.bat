sqlite3.exe ..\Application\SparkServerLite\SparkServer.db < 10_CreateAll.sql
sqlite3.exe ..\Application\SparkServerLite\SparkServer.db < 10_InsertTestData.sql

sqlite3.exe ..\Application\SparkServerLite\SparkServerAnalytics.db < 20_CreateAnalytics.sql