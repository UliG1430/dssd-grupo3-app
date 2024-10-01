
INSERT INTO public."Usuarios" ("Id","Username","Password","Email") VALUES
	 ('35436f4a-af5e-4ee4-a45e-66796cb784f9'::uuid,'asd.asd','PWG6/wyX4krcOsoy/gWYocT9aauIT+GE7qDNFoYMZPY=','asd');

INSERT INTO public."Niveles" ("Id","Nombre") VALUES
	 ('61ba8cc1-978f-4147-a68a-8f4cb530bd81'::uuid,'Primer Nivel'),
	 ('f24675dd-be06-4a8e-9ddb-a1a1654b4934'::uuid,'Segundo Nivel');
	
INSERT INTO public."Facturadores" ("Id","Nombre","Direccion","CuitPrimario","CuitFacturador","EsEstablecimiento","ProfesionalId") VALUES
	 ('5113d0fa-f439-41c9-972a-90389fafe4bd'::uuid,'Facultad de Informática UNLP','50 y 120','11-11111111-1','11-11111111-1',false,'72ba8f42-e68d-4f2a-b845-7e8217d01b3b'::uuid),
	 ('777f2bf0-b99d-467d-8325-bcd7798b888e'::uuid,'ITinfinit','45 e/ 4 y 5','00-00000000-0','00-00000000-0',true,NULL);
	
INSERT INTO public."UsuariosNivelFacturador" ("Id","UsuarioId","FacturadorId","NivelId") VALUES
	 ('4e4843d8-2c4b-4684-b60e-7aa70805aa4b'::uuid,'35436f4a-af5e-4ee4-a45e-66796cb784f9'::uuid,'777f2bf0-b99d-467d-8325-bcd7798b888e'::uuid,'61ba8cc1-978f-4147-a68a-8f4cb530bd81'::uuid),
	 ('5d51bfb5-4b8c-4831-aaa5-ed955a0211a9'::uuid,'35436f4a-af5e-4ee4-a45e-66796cb784f9'::uuid,'5113d0fa-f439-41c9-972a-90389fafe4bd'::uuid,'61ba8cc1-978f-4147-a68a-8f4cb530bd81'::uuid),
	 ('20f63882-876b-41b1-bbe0-80cb4bce9d55'::uuid,'35436f4a-af5e-4ee4-a45e-66796cb784f9'::uuid,'5113d0fa-f439-41c9-972a-90389fafe4bd'::uuid,'f24675dd-be06-4a8e-9ddb-a1a1654b4934'::uuid),
	 ('12a3f782-4671-4bff-98d7-0ce4ed020eb4'::uuid,'35436f4a-af5e-4ee4-a45e-66796cb784f9'::uuid,'777f2bf0-b99d-467d-8325-bcd7798b888e'::uuid,'f24675dd-be06-4a8e-9ddb-a1a1654b4934'::uuid);
