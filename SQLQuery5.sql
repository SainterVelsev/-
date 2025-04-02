ALTER TABLE Заказ
ADD CONSTRAINT Checker_Order
       CHECK (Статус NOT LIKE '%[^А-Яа-яЁё]%'AND 
			  Счёт NOT LIKE '%[^0-9.]%'AND
			  Номер_столика NOT LIKE '%[^0-9]%');
