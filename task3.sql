# Used MySQL dialect

# Tables

CREATE TABLE Employee (
  Id int NOT NULL AUTO_INCREMENT,
  Name varchar(255) NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE Turnstile (
  Id int NOT NULL AUTO_INCREMENT,
  EmployeeId int NOT NULL,
  EnterTime DATETIME NOT NULL,
  LeaveTime DATETIME,
  PRIMARY KEY (Id),
  FOREIGN KEY (EmployeeId) REFERENCES Employee(Id)
);

# Query with params

SELECT 
	e.Name,       
	SEC_TO_TIME(SUM(TIME_TO_SEC(t.LeaveTime) - TIME_TO_SEC(t.EnterTime))) AS TotalTime
FROM Employee e, Turnstile t
WHERE e.Id = t.EmployeeId AND t.LeaveTime IS NOT NULL AND t.EnterTime > @paramFrom AND t.LeaveTime < @paramTo
GROUP BY t.EmployeeId