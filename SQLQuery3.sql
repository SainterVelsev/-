ALTER TABLE Блюдо
ADD CONSTRAINT Checker_Food
       CHECK (Название NOT LIKE '%[^A-Za-zА-Яа-яЁё ]%');
