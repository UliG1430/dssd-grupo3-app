INSERT INTO public."ObrasSociales" ("Id","Nombre","Activa") VALUES
	 ('593e6cce-b89e-4821-95b8-845d26727996'::uuid,'IOMA',true),
	 ('ac049670-bf2a-4503-a6e3-9feb5e874c9d'::uuid,'OSDE',false),
	 ('4383c4e2-d12a-482a-bed8-a588dfdd9991'::uuid,'Swiss Medical',false),
	 ('a96da8bf-c152-4d59-8f1d-a1cb2c7daea1'::uuid,'PAMI',true);

INSERT INTO public."Profesionales" ("Id","Cuit","MatriculaProvincial","MatriculaNacional","Apellidos","Nombres","Domicilio","Telefono","Email","FechaNacimiento") VALUES
	 ('72ba8f42-e68d-4f2a-b845-7e8217d01b3b'::uuid,'27443563291','123456','654321','Zuppa Moro','Catalina',NULL,NULL,NULL,'2002-09-22 21:00:00-03'),
	 ('6408d45a-99f8-4182-9744-6b11ed0e0811'::uuid,'11445556661','987654','456789','Borrelli Zara','Juana',NULL,NULL,NULL,'2002-09-13 12:09:04.418-03'),
	 ('800250f6-7d0f-4d6f-810b-6bc166ef3dd9'::uuid,'11334445551','876543','345678','Arp','Pablo Damián',NULL,NULL,NULL,'1994-09-13 12:18:11.715-03'),
	 ('3d033fb8-58b9-48fb-a638-c8564aec65ae'::uuid,'11223334441','765432','234567','Stiro','Natalia',NULL,NULL,NULL,'1994-09-13 12:18:11.715-03');

INSERT INTO public."ProfesionalesFacturador" ("Id","ProfesionalId","FacturadorId","FechaDesde","FechaHasta") VALUES
	 ('10e26334-c1d1-4469-9854-b05db87995bf'::uuid,'6408d45a-99f8-4182-9744-6b11ed0e0811'::uuid,'777f2bf0-b99d-467d-8325-bcd7798b888e'::uuid,'2024-09-13 12:12:40.42189-03',NULL),
	 ('4da9ae4e-9b7c-4b80-bba5-fe2b5e1df9ae'::uuid,'6408d45a-99f8-4182-9744-6b11ed0e0811'::uuid,'5113d0fa-f439-41c9-972a-90389fafe4bd'::uuid,'2024-09-13 12:13:17.661226-03',NULL),
	 ('e1f28d2e-2dc1-43b9-b480-6a925ca62fc4'::uuid,'800250f6-7d0f-4d6f-810b-6bc166ef3dd9'::uuid,'5113d0fa-f439-41c9-972a-90389fafe4bd'::uuid,'2024-09-13 12:20:17.246329-03',NULL),
	 ('ab4e626f-fb21-469e-9e41-01e14b2ccc70'::uuid,'3d033fb8-58b9-48fb-a638-c8564aec65ae'::uuid,'777f2bf0-b99d-467d-8325-bcd7798b888e'::uuid,'2024-09-13 12:22:03.677252-03',NULL),
	 ('8d36753c-29d0-4944-a2b5-ebd279b3040a'::uuid,'72ba8f42-e68d-4f2a-b845-7e8217d01b3b'::uuid,'777f2bf0-b99d-467d-8325-bcd7798b888e'::uuid,'2024-09-13 12:22:45.942532-03',NULL),
	 ('dbc7aaff-4785-4b1d-83ff-fbe364b76880'::uuid,'72ba8f42-e68d-4f2a-b845-7e8217d01b3b'::uuid,'5113d0fa-f439-41c9-972a-90389fafe4bd'::uuid,'2024-09-13 12:22:58.326932-03',NULL);