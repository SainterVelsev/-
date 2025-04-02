ALTER TABLE ТТК
ADD CONSTRAINT Checker_ttk
       CHECK (количество_ингредиентов NOT LIKE '%[^0-9]%' AND 
			  Единица_измерения NOT LIKE '%[^А-Яа-яЁё]%');