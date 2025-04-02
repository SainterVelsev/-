
ALTER TABLE Склад
ADD CONSTRAINT Checker_Stor
       CHECK (Название_склада NOT LIKE '%[^А-Яа-яЁё ]%');
