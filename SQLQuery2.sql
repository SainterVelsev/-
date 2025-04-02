ALTER TABLE Сотрудник
ADD CONSTRAINT Checker_Empl
       CHECK (Имя NOT LIKE '%[^A-Za-zА-Яа-яЁё]%'AND 
              Отчество NOT LIKE '%[^A-Za-zА-Яа-яЁё]%' AND 
              Фамилия NOT LIKE '%[^A-Za-zА-Яа-яЁё]%' AND
			  Телефон NOT LIKE '^\+7[0-9]{10}$');




