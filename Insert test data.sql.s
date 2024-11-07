INSERT INTO "Events" ("Id", "Name", "Location", "Date", "AvailableTickets") VALUES
(1, 'Perez, Johnson and Martin', 'West Michael', '2022-10-27', 175),
(2, 'Perez-Booker', 'Port Crystal', '2020-06-17', 105),
(3, 'Jones-Good', 'Port Thomasmouth', '2020-07-21', 154),
(4, 'Kemp, Kelly and Francis', 'Garzaville', '2023-08-18', 91),
(5, 'Gaines LLC', 'Josephville', '2021-11-20', 117);



INSERT INTO "Bookings" ("Id", "EventId", "NumberOfTickets", "BookingDate") VALUES
(1, 3, 2, '2024-01-02 00:00:00'),
(2, 3, 4, '2024-04-22 00:00:00'),
(3, 4, 1, '2024-10-01 00:00:00'),
(4, 1, 5, '2024-10-04 00:00:00'),
(5, 2, 3, '2024-03-29 00:00:00'),
(6, 3, 4, '2024-01-21 00:00:00'),
(7, 5, 1, '2024-09-20 00:00:00'),
(8, 5, 2, '2024-09-03 00:00:00'),
(9, 1, 1, '2024-07-27 00:00:00'),
(10, 5, 2, '2024-10-25 00:00:00');
