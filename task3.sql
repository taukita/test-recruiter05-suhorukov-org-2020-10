CREATE TABLE Employee (
  Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
  Name varchar(255) NOT NULL
);

CREATE TABLE Turnstile (
  Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
  EmployeeId int NOT NULL,
  EnterTime DATETIME NOT NULL,
  LeaveTime DATETIME,
  FOREIGN KEY (EmployeeId) REFERENCES Employee(Id)
);

# Query with params

SELECT 
	e.Name,
    ISNULL((SELECT SUM(DATEDIFF (second, t.EnterTime, ISNULL(t.LeaveTime, GETDATE())) / (60.0 * 60.0))
	FROM Turnstile t
	WHERE e.Id = t.EmployeeId AND t.EnterTime > @paramFrom AND (t.LeaveTime < @paramTo OR t.LeaveTime IS NULL)),0) AS TotalHours
FROM Employee e