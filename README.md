# Diplomski-projekat
 
docker-compose exec mysql mysql –u root 

DROP DATABASE IF EXIST vezba;
CREATE DATABASE vezba; 
USE vezba;

create table kupac (
  kupac_id int primary key,
  ime_kupca varchar(20),
  prezime varchar(20),
  jmbg varchar(13)
);
create table proizvod(
  proizvod_id int primary key,
  ime_proizvoda varchar(20),
  cena int
);
create table kupovina(
  kupovina_id int primary key,
  id_proizvod int,
  id_kupac int,
  kolicina int
);

---------------------------------------------------------

docker-compose exec mysql mysql –h tidb –u root –P 4000

DROP DATABASE IF EXISTS vezba;
CREATE DATABASE vezba; 
USE vezba;

create table transakcija(
    transakcija_id int primary key,
    potrosnja int,
    ime varchar(20),
    prezime varchar(20),
    jmbg varchar(13),
    proizvod varchar(20),
    datum TIMESTAMP 
);

------------------------------------------------------
Flink 
docker-compose exec jobmanager ./bin/sql-client.sh embedded -l ./connector-lib


create table kupac (
  kupac_id int primary key,
  ime_kupca varchar(20),
  prezime varchar(20),
  jmbg varchar(13)
) WITH (
    'connector' = 'mysql-cdc',
    'hostname' = 'mysql',
    'port' = '3306',
    'username' = 'root',
    'password' = '',
    'database-name' = 'vezba',
    'table-name' = 'kupac'
);

create table proizvod(
  proizvod_id int primary key,
  ime_proizvoda varchar(20),
  cena int
) WITH (
    'connector' = 'mysql-cdc',
    'hostname' = 'mysql',
    'port' = '3306',
    'username' = 'root',
    'password' = '',
    'database-name' = 'vezba',
    'table-name' = 'proizvod'
);

create table kupovina(
  kupovina_id int primary key,
  id_proizvod int,
  id_kupac int,
  kolicina int
) WITH (
    'connector' = 'mysql-cdc',
    'hostname' = 'mysql',
    'port' = '3306',
    'username' = 'root',
    'password' = '',
    'database-name' = 'vezba',
    'table-name' = 'kupovina'
);

create table transakcija( 
    transakcija_id int primary key,    
    potrosnja int,
    ime varchar(20),
    prezime varchar(20),
    jmbg varchar(13),
    proizvod varchar(20),
    datum TIMESTAMP 
) WITH (
    'connector'  = 'jdbc',
    'driver'     = 'com.mysql.cj.jdbc.Driver',
    'url'        = 'jdbc:mysql://tidb:4000/vezba?rewriteBatchedStatements=true',
    'table-name' = 'transakcija',
    'username'   = 'root',
    'password'   = ''
);



INSERT INTO transakcija
SELECT
    MAX(k.kupovina_id) AS transakcija_id,
    SUM(k.kolicina * pr.cena) AS potrosnja,
    ku.ime_kupca AS ime,
    ku.prezime,
    ku.jmbg,
    pr.ime_proizvoda AS proizvod,
    NOW() AS datum
FROM
    kupovina AS k
INNER JOIN kupac AS ku ON ku.kupac_id = k.id_kupac
INNER JOIN proizvod AS pr ON pr.proizvod_id = k.id_proizvod
GROUP BY ku.ime_kupca, ku.prezime, ku.jmbg, pr.ime_proizvoda;

------------------------------------------------------------------

insert into proizvod values (1, 'Proizvod1', 10000);
insert into proizvod values (2, 'Proizvod2', 4000);
insert into proizvod values (3, 'Proizvod3', 2000);

insert into kupac values (1, 'Petar', 'Petrovic','2806998979813');
insert into kupac values (2, 'Janko', 'Jankovic','1303200212345');
insert into kupac values (3, 'Nemanja', 'Pavic','1234567891234');
insert into kupac values (4, 'Nebojsa', 'Rakic','0107567891234');

insert into kupovina values (1, 1, 1, 2);

