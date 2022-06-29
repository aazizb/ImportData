# SQL Test Assignment

Attached is a mysqldump of a database to be used during the test.

Below are the questions for this test. Please enter a full, complete, working SQL statement under each question. We do not want the answer to the question. We want the SQL command to derive the answer. We will copy/paste these commands to test the validity of the answer.

**Example:**

_Q. Select all users_

- Please return at least first_name and last_name

SELECT first_name, last_name FROM users;


------

**— Test Starts Here —**

1. Select users whose id is either 3,2 or 4
- Please return at least: all user fields

SELECT 
    id, first_name, last_name, email, status, created
FROM
    users
WHERE
    id IN (3 , 2, 4);
	
2. Count how many basic and premium listings each active user has
- Please return at least: first_name, last_name, basic, premium

SELECT 
    lst.user_id,
    usr.first_name,
    usr.last_name,
    SUM(CASE
        WHEN lst.status = 2 THEN 1
        ELSE 0
    END) AS basic,
    SUM(CASE
        WHEN lst.status = 3 THEN 1
        ELSE 0
    END) AS premium
FROM
    listings AS lst
        INNER JOIN
    users AS usr ON lst.user_id = usr.id AND usr.status = 2
GROUP BY user_id;

3. Show the same count as before but only if they have at least ONE premium listing
- Please return at least: first_name, last_name, basic, premium

SELECT 
    lst.user_id,
    usr.first_name,
    usr.last_name,
    SUM(CASE
        WHEN lst.status = 2 THEN 1
        ELSE 0
    END) AS basic,
    SUM(CASE
        WHEN lst.status = 3 THEN 1
        ELSE 0
    END) AS premium
FROM
    listings AS lst
        INNER JOIN
    users AS usr ON lst.user_id = usr.id AND usr.status = 2
GROUP BY user_id
HAVING premium > 0;

4. How much revenue has each active vendor made in 2013
- Please return at least: first_name, last_name, currency, revenue

SELECT 
    usr.id, usr.first_name, usr.last_name, clk.currency, SUM(clk.price)
FROM
    users AS usr
        INNER JOIN
    listings AS lst ON usr.id = lst.user_id
        LEFT JOIN
    clicks AS clk ON lst.id = clk.listing_id
WHERE
    YEAR(clk.created) = 2013 AND usr.status = 2
GROUP BY  clk.currency;

5. Insert a new click for listing id 3, at $4.00
- Find out the id of this new click. Please return at least: id

START TRANSACTION;
  INSERT INTO clicks (listing_id, price, created) VALUES (3, 4.00, Current_timestamp());
SELECT 
    id, listing_id, price, currency, created
FROM
    clicks
WHERE
    id = (SELECT LAST_INSERT_ID());
COMMIT;

6. Show listings that have not received a click in 2013
- Please return at least: listing_name

SELECT 
    lst.id, lst.user_id, lst.name, lst.status
FROM
    listings AS lst
WHERE
    NOT EXISTS( SELECT 
            1
        FROM
            clicks clk
        WHERE
            clk.listing_id = lst.id
                AND YEAR(created) = 2013);

7. For each year show number of listings clicked and number of vendors who owned these listings
- Please return at least: date, total_listings_clicked, total_vendors_affected

SELECT 
    DATE(clk.created) AS Date,
    COUNT(DISTINCT lst.id) AS total_listings_clicked,
    COUNT(DISTINCT usr.id) AS total_vendors_affected
FROM
    listings AS lst
        INNER JOIN
    users AS usr ON lst.user_id = usr.id
        INNER JOIN
    clicks AS clk ON lst.id = clk.listing_id
GROUP BY clk.created;

8. Return a comma separated string of listing names for all active vendors
- Please return at least: first_name, last_name, listing_names

SELECT 
    'id',
    'first_name',
    'last_name',
    'email',
    'status',
    'created'

UNION SELECT 
    id, first_name, last_name, email, status, created
FROM
    users
WHERE
    status = 2 INTO OUTFILE 'C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/csvfile.csv' FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '"' LINES TERMINATED BY '
';