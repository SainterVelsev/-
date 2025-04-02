ALTER TABLE Ингредиенты_для_блюда
ADD CONSTRAINT Checker_Ing
       CHECK (название NOT LIKE '%[^A-Za-zА-Яа-яЁё ]%'AND
			  единица_измерения NOT LIKE '%[^A-Za-zА-Яа-яЁё ]%'AND
			  цена NOT LIKE '%[^0-9.]%');
