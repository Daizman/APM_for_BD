--
-- PostgreSQL database dump
--

-- Dumped from database version 10.6
-- Dumped by pg_dump version 10.6

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: helpers; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA helpers;


ALTER SCHEMA helpers OWNER TO postgres;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: fields; Type: TABLE; Schema: helpers; Owner: postgres
--

CREATE TABLE helpers.fields (
    id integer NOT NULL,
    field_name character varying(255) NOT NULL,
    table_name character varying(255) NOT NULL,
    field_type character varying(255) NOT NULL,
    transl_fn character varying(255) NOT NULL,
    category_name character varying(255) NOT NULL
);


ALTER TABLE helpers.fields OWNER TO postgres;

--
-- Name: fields_id_seq; Type: SEQUENCE; Schema: helpers; Owner: postgres
--

CREATE SEQUENCE helpers.fields_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE helpers.fields_id_seq OWNER TO postgres;

--
-- Name: fields_id_seq; Type: SEQUENCE OWNED BY; Schema: helpers; Owner: postgres
--

ALTER SEQUENCE helpers.fields_id_seq OWNED BY helpers.fields.id;


--
-- Name: rel_table; Type: TABLE; Schema: helpers; Owner: postgres
--

CREATE TABLE helpers.rel_table (
    id integer NOT NULL,
    table1 character varying(255) NOT NULL,
    table2 character varying(255) NOT NULL,
    relations character varying(500),
    via character varying(255),
    CONSTRAINT rel_table_check CHECK (((relations IS NULL) <> (via IS NULL)))
);


ALTER TABLE helpers.rel_table OWNER TO postgres;

--
-- Name: rel_table_id_seq; Type: SEQUENCE; Schema: helpers; Owner: postgres
--

CREATE SEQUENCE helpers.rel_table_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE helpers.rel_table_id_seq OWNER TO postgres;

--
-- Name: rel_table_id_seq; Type: SEQUENCE OWNED BY; Schema: helpers; Owner: postgres
--

ALTER SEQUENCE helpers.rel_table_id_seq OWNED BY helpers.rel_table.id;


--
-- Name: content; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.content (
    id integer NOT NULL,
    c_type character varying(50) NOT NULL,
    name character varying(50) NOT NULL,
    country character varying(50) NOT NULL,
    release_date date NOT NULL,
    budget integer NOT NULL,
    duration interval NOT NULL,
    description character varying(1000) NOT NULL,
    dub character varying(750) NOT NULL
);


ALTER TABLE public.content OWNER TO postgres;

--
-- Name: content_for_site; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.content_for_site (
    add_date date NOT NULL,
    site_id integer NOT NULL,
    content_id integer NOT NULL,
    id integer NOT NULL
);


ALTER TABLE public.content_for_site OWNER TO postgres;

--
-- Name: content_for_site_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.content_for_site_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.content_for_site_id_seq OWNER TO postgres;

--
-- Name: content_for_site_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.content_for_site_id_seq OWNED BY public.content_for_site.id;


--
-- Name: content_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.content_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.content_id_seq OWNER TO postgres;

--
-- Name: content_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.content_id_seq OWNED BY public.content.id;


--
-- Name: news; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.news (
    id integer NOT NULL,
    header character varying(50) NOT NULL,
    content character varying(1500) NOT NULL,
    release_date date NOT NULL,
    site_id integer NOT NULL
);


ALTER TABLE public.news OWNER TO postgres;

--
-- Name: news_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.news_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.news_id_seq OWNER TO postgres;

--
-- Name: news_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.news_id_seq OWNED BY public.news.id;


--
-- Name: site; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.site (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    addr character varying(230) NOT NULL,
    access boolean NOT NULL,
    description character varying(1500) NOT NULL
);


ALTER TABLE public.site OWNER TO postgres;

--
-- Name: site_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.site_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.site_id_seq OWNER TO postgres;

--
-- Name: site_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.site_id_seq OWNED BY public.site.id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    password character varying NOT NULL,
    password_byte bytea NOT NULL,
    salt character varying NOT NULL,
    role_is_admin boolean NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_id_seq OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- Name: fields id; Type: DEFAULT; Schema: helpers; Owner: postgres
--

ALTER TABLE ONLY helpers.fields ALTER COLUMN id SET DEFAULT nextval('helpers.fields_id_seq'::regclass);


--
-- Name: rel_table id; Type: DEFAULT; Schema: helpers; Owner: postgres
--

ALTER TABLE ONLY helpers.rel_table ALTER COLUMN id SET DEFAULT nextval('helpers.rel_table_id_seq'::regclass);


--
-- Name: content id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content ALTER COLUMN id SET DEFAULT nextval('public.content_id_seq'::regclass);


--
-- Name: content_for_site id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_for_site ALTER COLUMN id SET DEFAULT nextval('public.content_for_site_id_seq'::regclass);


--
-- Name: news id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news ALTER COLUMN id SET DEFAULT nextval('public.news_id_seq'::regclass);


--
-- Name: site id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.site ALTER COLUMN id SET DEFAULT nextval('public.site_id_seq'::regclass);


--
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- Data for Name: fields; Type: TABLE DATA; Schema: helpers; Owner: postgres
--

INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (1, 'c_type', 'content', 'varchar(50)', 'Тип контента', 'Контент');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (2, 'name', 'content', 'varchar(50)', 'Название контента', 'Контент');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (3, 'country', 'content', 'varchar(50)', 'Страна производитель', 'Контент');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (4, 'release_date', 'content', 'date', 'Дата выпуска', 'Контент');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (5, 'budget', 'content', 'int4', 'Бюджет', 'Контент');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (6, 'duration', 'content', 'interval', 'Длительность', 'Контент');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (7, 'description', 'content', 'varchar(1000)', 'Описание', 'Контент');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (8, 'dub', 'content', 'varchar(750)', 'Дубляж', 'Контент');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (9, 'add_date', 'content_for_site', 'date', 'Дата добавления на сайт', 'Контент для сайта');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (10, 'header', 'news', 'varchar(50)', 'Заголовок', 'Новость');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (11, 'content', 'news', 'varchar(1500)', 'Контент', 'Новость');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (12, 'release_date', 'news', 'date', 'Дата появления', 'Новость');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (13, 'name', 'site', 'varchar(50)', 'Название', 'Сайт');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (14, 'addr', 'site', 'varchar(230)', 'Адрес', 'Сайт');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (15, 'access', 'site', 'bool', 'Доступ', 'Сайт');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (16, 'description', 'site', 'varchar(1500)', 'Описание', 'Сайт');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (17, 'name', 'users', 'varchar(50)', 'Имя', 'Пользователи');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (18, 'password', 'users', 'varchar', 'Пароль', 'Пользователи');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (19, 'salt', 'users', 'varcahr', 'Соль', 'Пользователи');
INSERT INTO helpers.fields (id, field_name, table_name, field_type, transl_fn, category_name) VALUES (20, 'role_is_admin', 'users', 'bool', 'Является админом', 'Пользователи');


--
-- Data for Name: rel_table; Type: TABLE DATA; Schema: helpers; Owner: postgres
--

INSERT INTO helpers.rel_table (id, table1, table2, relations, via) VALUES (1, 'site', 'news', 'news.site_id = site.id', NULL);
INSERT INTO helpers.rel_table (id, table1, table2, relations, via) VALUES (2, 'site', 'content_for_site', 'content_for_site.site_id = site.id', NULL);
INSERT INTO helpers.rel_table (id, table1, table2, relations, via) VALUES (3, 'content', 'content_fo_site', 'content_for_site.content_id = content.id', NULL);
INSERT INTO helpers.rel_table (id, table1, table2, relations, via) VALUES (4, 'site', 'content', NULL, 'content_for_site');


--
-- Data for Name: content; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.content (id, c_type, name, country, release_date, budget, duration, description, dub) VALUES (6, '1', '1', '1', '0001-01-01', 0, '00:00:00', '1', '1');
INSERT INTO public.content (id, c_type, name, country, release_date, budget, duration, description, dub) VALUES (13, '''drop table *', '''drop table *', '''drop table *', '0001-01-01', 0, '00:00:00', '''drop table *', '''drop table *');
INSERT INTO public.content (id, c_type, name, country, release_date, budget, duration, description, dub) VALUES (7, '2', '1', '2', '0001-01-01', 0, '00:00:00', '2', '2');


--
-- Data for Name: content_for_site; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.content_for_site (add_date, site_id, content_id, id) VALUES ('2019-04-21', 16, 6, 36);
INSERT INTO public.content_for_site (add_date, site_id, content_id, id) VALUES ('2019-04-21', 15, 7, 40);
INSERT INTO public.content_for_site (add_date, site_id, content_id, id) VALUES ('2019-04-21', 16, 7, 41);
INSERT INTO public.content_for_site (add_date, site_id, content_id, id) VALUES ('2019-04-21', 16, 13, 42);
INSERT INTO public.content_for_site (add_date, site_id, content_id, id) VALUES ('2019-04-21', 15, 13, 43);
INSERT INTO public.content_for_site (add_date, site_id, content_id, id) VALUES ('2019-04-21', 17, 13, 44);


--
-- Data for Name: news; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.news (id, header, content, release_date, site_id) VALUES (10, '''drop table *', '''drop table *', '0001-01-01', 15);
INSERT INTO public.news (id, header, content, release_date, site_id) VALUES (12, 'vihoda net', 'vihoda net', '0001-01-01', 19);
INSERT INTO public.news (id, header, content, release_date, site_id) VALUES (14, 'опять на работу', 'я как машина', '1337-09-11', 18);
INSERT INTO public.news (id, header, content, release_date, site_id) VALUES (13, 'выхода нет', 'только ДжоДжо', '1313-12-12', 19);


--
-- Data for Name: site; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.site (id, name, addr, access, description) VALUES (15, '''drop table *', '''drop table *', false, '''drop table *');
INSERT INTO public.site (id, name, addr, access, description) VALUES (17, 'aaaaaa', 'help', false, 'aaaaaa');
INSERT INTO public.site (id, name, addr, access, description) VALUES (18, 'pomogite', 'pomogite', false, 'pomogite');
INSERT INTO public.site (id, name, addr, access, description) VALUES (19, 'helpMe', 'helpMe', false, 'helpMe');
INSERT INTO public.site (id, name, addr, access, description) VALUES (16, 'ddd', 'ddd', true, 'ddd');


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.users (id, name, password, password_byte, salt, role_is_admin) VALUES (16, 'слабый юзер', '1337', '\x8c7d378fd3405997c1cd19cc2ea44b752ba37cb320d370a8619758b5e73b3270', '965869867172570266768672874674769272371069270573867473573971470771770165269', false);
INSERT INTO public.users (id, name, password, password_byte, salt, role_is_admin) VALUES (17, 'сильный юзер', '987', '\xfbd839bf6abddcac217f7852e7677be857b61c09b0b3da3b600a214a4e294276', '874271365270267965073067968372572873774767366869574471768269374974769568068', true);
INSERT INTO public.users (id, name, password, password_byte, salt, role_is_admin) VALUES (13, '''drop table *', '''drop table *', '\x30f978d2f3e6f817d4be2c44a426f46286ec559797268c4465448d670cc87688', '870968266173965373565771968769868072970074072570665674867670766865665869369', false);
INSERT INTO public.users (id, name, password, password_byte, salt, role_is_admin) VALUES (18, '1', '123456789', '\x750d59c1796a8f4700cddc2f8cc8ccfd43f8bd2b549f08b04f570471221ae712', '673273266068674268468366267672267568668972668565473271273569974774971570572', true);


--
-- Name: fields_id_seq; Type: SEQUENCE SET; Schema: helpers; Owner: postgres
--

SELECT pg_catalog.setval('helpers.fields_id_seq', 20, true);


--
-- Name: rel_table_id_seq; Type: SEQUENCE SET; Schema: helpers; Owner: postgres
--

SELECT pg_catalog.setval('helpers.rel_table_id_seq', 4, true);


--
-- Name: content_for_site_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.content_for_site_id_seq', 44, true);


--
-- Name: content_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.content_id_seq', 13, true);


--
-- Name: news_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.news_id_seq', 14, true);


--
-- Name: site_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.site_id_seq', 19, true);


--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 18, true);


--
-- Name: fields fields_pk; Type: CONSTRAINT; Schema: helpers; Owner: postgres
--

ALTER TABLE ONLY helpers.fields
    ADD CONSTRAINT fields_pk PRIMARY KEY (id);


--
-- Name: rel_table rel_table_pk; Type: CONSTRAINT; Schema: helpers; Owner: postgres
--

ALTER TABLE ONLY helpers.rel_table
    ADD CONSTRAINT rel_table_pk PRIMARY KEY (id);


--
-- Name: content_for_site content_for_site_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_for_site
    ADD CONSTRAINT content_for_site_pk PRIMARY KEY (id);


--
-- Name: content_for_site content_for_site_un; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_for_site
    ADD CONSTRAINT content_for_site_un UNIQUE (site_id, content_id);


--
-- Name: content content_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content
    ADD CONSTRAINT content_pk PRIMARY KEY (id);


--
-- Name: news news_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news
    ADD CONSTRAINT news_pk PRIMARY KEY (id);


--
-- Name: site site_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.site
    ADD CONSTRAINT site_pk PRIMARY KEY (id);


--
-- Name: users users_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pk PRIMARY KEY (id);


--
-- Name: content_for_site_id_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX content_for_site_id_idx ON public.content_for_site USING btree (id);


--
-- Name: content_id_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX content_id_idx ON public.content USING btree (id);


--
-- Name: news_id_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX news_id_idx ON public.news USING btree (id);


--
-- Name: site_id_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX site_id_idx ON public.site USING btree (id);


--
-- Name: users_id_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX users_id_idx ON public.users USING btree (id);


--
-- Name: content_for_site content_for_site_content_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_for_site
    ADD CONSTRAINT content_for_site_content_fk FOREIGN KEY (content_id) REFERENCES public.content(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: content_for_site content_for_site_site_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_for_site
    ADD CONSTRAINT content_for_site_site_fk FOREIGN KEY (site_id) REFERENCES public.site(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: news news_site_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.news
    ADD CONSTRAINT news_site_fk FOREIGN KEY (site_id) REFERENCES public.site(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

