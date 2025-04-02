ALTER TABLE Должности
ADD CONSTRAINT Checker_Rol
       CHECK (Название_должности NOT LIKE '%[^А-Яа-яЁё]%');
