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




insert into proizvod values (1, 'Proizvod1', 10000);
insert into proizvod values (2, 'Proizvod2', 4000);
insert into proizvod values (3, 'Proizvod3', 2000);

insert into kupac values (1, 'Petar', 'Petrovic','2806998979813');
insert into kupac values (2, 'Janko', 'Jankovic','1303200212345');
insert into kupac values (3, 'Nemanja', 'Pavic','1234567891234');
insert into kupac values (4, 'Nebojsa', 'Rakic','0107567891234');

insert into kupovina values (1, 1, 1, 2);

