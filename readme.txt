- Oracle_Linux6
- setup primary
-- copy oracle12c to share

// set ORACLE_SID name orcl, path = /u01/app/oracle/product/12.1.0/dbhome_1
. oraenv 

// start listener
lsnrctl start

// start database splplus
sqlplus / as sysdba	

