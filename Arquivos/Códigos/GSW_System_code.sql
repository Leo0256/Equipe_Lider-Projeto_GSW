create database "GSWSystem"
    with 
    owner = postgres
    encoding = 'UTF8'
    lc_collate = 'Portuguese_Brazil.1252'
    lc_ctype = 'Portuguese_Brazil.1252'
    tablespace = pg_default
    connection limit = -1;


create table funcionarios(
    id_func varchar(40) primary key,
    primeiro_nome varchar(50) not null,
    ultimo_nome varchar(50) not null,
    avatar varchar(100) default 'http://placeimg.com/640/480/people',
    email varchar(100) not null
)


create table gitmetadata(
    id_git serial primary key,
    branch varchar(50) not null,
    hash varchar(50) unique not null
)


create table projeto_info(
    id_info serial primary key,
    nome varchar(100) not null,
    descr varchar(200),
    horas numeric(4,2) default 0.0
)


create table projeto(
    id varchar(40) primary key,
    id_func varchar(40) not null,
    id_info integer not null,
    id_git integer not null,
    iniciado timestamp,
    status varchar(20),
    finalizado integer default 0,
    foreign key (id_func) references funcionarios (id_func),
    foreign key (id_info) references projeto_info (id_info),
    foreign key (id_git) references gitmetadata (id_git)
)


create or replace function inserir_func(func json)
returns boolean
language plpgsql
as $$
declare
	refFunc varchar := func::json->>'id_func';
begin
	case when 
		(select (count(id_func)::int)::boolean from funcionarios where id_func like refFunc)
	then
		with jsonFunc as (
			select * from json_populate_recordset(
				null::funcionarios,
				func)
		)
		update funcionarios
		set
			primeiro_nome = a.primeiro_nome,
			ultimo_nome = a.ultimo_nome,
			avatar = a.avatar,
			email = a.email
		from
			jsonFunc a
		where
			id_func = refFunc;
		
	else
		insert into funcionarios values
		(
			refFunc,
			func::json->>'primeiro_nome',
			func::json->>'ultimo_nome',
			func::json->>'avatar',
			func::json->>'email'
		);
		
	end case;
	
	return (select (count(id_func)::int)::boolean from funcionarios where id_func like refFunc);
end $$;


create or replace function inserir_projInfo(projInfo json)
returns boolean
language plpgsql
as $$
declare
	refInfo varchar := projInfo::json->>'nome';
begin
	case when
		(select (count(nome)::int)::boolean from projeto_info where nome like refInfo)
	then
		with jsonInfo as (
			select * from json_populate_recordset(
				null::projeto_info,
				projInfo)
		)
		update projeto_info
		set
			descr = a.descr,
			horas = a.horas
		from jsonInfo a
		where
			nome = refInfo;
		
	else
		insert into projeto_info values
		(
			refInfo,
			projInfo::json->>'descr',
			projInfo::json->>'horas'
		);
	
	end case;
	
	return (select (count(nome)::int)::boolean from projeto_info where nome like refInfo);
end $$;


create or replace function inserir_git(git json)
returns boolean
language plpgsql
as $$
declare
	refGit varchar := git::json->>'hash';
begin
	case when 
		(select (count(hash)::int)::boolean from gitmetadata where hash like refGit)
	then
		with jsonGit as (
			select * from json_populate_recordset(
				null::gitmetadata,
				func)
		)
		update gitmetadata
		set
			branch = a.branch
		from
			jsonGit a
		where
			hash = refGit;
		
	else
		insert into gitmetadata values
		(
			git::json->>'branch',
			refGit
		);
		
	end case;
	
	return (select (count(hash)::int)::boolean from gitmetadata where hash like refGit);
end $$;


/*
create or replace function inserir_dados(
	func json,
	projInfo json,
	git json,
	projeto json
) 
returns void
as $$
begin
	
	insert into funcionarios(id_func,primeiro_nome,ultimo_nome,email)
		values('<novo>','<novo>','<novo>','<novo>');
	
	insert into projeto_info(nome) values('<novo>');
	
	insert into gitmetadata(branch,hash) values('<novo>','<novo>');
	
	insert into projeto(id,id_func,id_info,id_git) 
		select 
			funcionarios.email,
			funcionarios.id_func,
			projeto_info.id_info,
			gitmetadata.id_git
		from funcionarios,projeto_info,gitmetadata
		where 
			funcionarios.id_func = '<novo>'
		and
			projeto_info.nome = '<novo>'
		and
			gitmetadata.hash = '<novo>';
		
	
	/*Funcionarios*
	with newFunc as (
		with jsonFunc as (
			select * from json_populate_recordset(
				null::funcionarios,
				func
			))
		update funcionarios
		set 
			id_func = a.id_func, 
			primeiro_nome = a.primeiro_nome,
			ultimo_nome = a.ultimo_nome,
			avatar = a.avatar,
			email = a.email

		from jsonFunc as a
		where funcionarios.id_func = '<novo>'
		returning a.id_func
		)
	update projeto
	set
		id_func = newFunc.id_func
	from newFunc
	where projeto.id = '<novo>';
	
	/*Projeto_info*
	with jsonProjInfo as (
		select * from json_populate_recordset(
			null::projeto_info,
			projInfo
		))
	update projeto_info
	set 
		nome = b.nome, 
		descr = b.descr,
		horas = b.horas
	from jsonProjInfo as b
	where projeto_info.nome = '<novo>';
	
	/*GitMetadata*
	with jsonGit as (
		select * from json_populate_recordset(
			null::gitmetadata,
			git
		))
	update gitmetadata
	set 
		branch = c.branch,
		hash = c.hash
	from jsonGit as c
	where gitmetadata.hash = '<novo>';
	
	/*Projeto*
	with jsonProj as (
		select * from json_populate_recordset(
			null::projeto,
			projeto
		))
	update projeto
	set 
		id = x.id,
		iniciado = x.iniciado::timestamp,
		status = x.status,
		finalizado = x.finalizado
	from jsonProj as x
	where projeto.id = '<novo>';
	
end $$
language plpgsql;
*/


create function pesquisa_dedicacao_func (nome_projeto varchar) 
returns table (
	id integer,
	projeto varchar,
	p_nome varchar,
	u_nome varchar,
	porcentagem double precision
) language plpgsql
as $$
begin
	return query 
		select
			projeto_info.id_info id,
			projeto_info.nome as projeto,
			funcionarios.primeiro_nome as p_nome,
			funcionarios.ultimo_nome as u_nome,
			sum(
				case
				when
					projeto_info.id_info = proj.id
				then
					((projeto_info.horas/proj.horas)*100)::float
				end
			) as valor

		from funcionarios 
			inner join projeto
				on funcionarios.id_func = projeto.id_func
			inner join projeto_info
				on projeto.id_info = projeto_info.id_info,

			(
				select
					projeto_info.id_info as id,
					sum(projeto_info.horas) as horas
				from projeto
					inner join projeto_info
						on projeto.id_info = projeto_info.id_info
				group by projeto_info.id_info
				order by projeto_info.id_info asc
			) proj
		where nome ilike concat('%',nome_projeto,'%')
		group by projeto_info.id_info,p_nome,u_nome
		order by id;
end; $$


create function pesquisa_finalizado_projeto (finalizado_x integer) 
returns table (
	id varchar,
	id_func varchar,
	nome varchar,
	decr varchar,
	iniciado timestamp,
	status varchar,
	finalizado integer
) language plpgsql
as $$
begin
	return query 
		select
			projeto.id, 
			projeto.id_func, 
			projeto_info.nome,
			projeto_info.descr,
			projeto.iniciado,
			projeto.status,
			projeto.finalizado
		from projeto, projeto_info
		where 
			projeto.id_info = projeto_info.id_info
		and
			projeto.finalizado = finalizado_x
		order by projeto.finalizado asc;

end; $$


create function pesquisa_horas_ano (ano_x integer) 
returns table (
	ano double precision,
	horas numeric(4,2)
) language plpgsql
as $$
begin
	return query 
		select
			extract(year from projeto.iniciado),
			sum(projeto_info.horas)

		from projeto_info
			inner join projeto 
				on projeto_info.id_info = projeto.id_info
				
		where 
			extract(year from projeto.iniciado)::integer = ano_x
		
		group by extract(year from projeto.iniciado)
		order by ano_x desc;

end; $$


create function pesquisa_horas_funcionario (func_nome varchar) 
returns table (
	id_func varchar,
	horas numeric(5,2),
	primeiro_nome varchar,
	ultimo_nome varchar,
	avatar varchar,
	email varchar
) language plpgsql
as $$
begin
	return query 
		select
			funcionarios.id_func,
			sum(projeto_info.horas) as horas,
			funcionarios.primeiro_nome,
			funcionarios.ultimo_nome,
			funcionarios.avatar,
			funcionarios.email
	
		from funcionarios
			inner join projeto 
				on funcionarios.id_func = projeto.id_func
			inner join projeto_info 
				on projeto.id_info = projeto_info.id_info

		where
			funcionarios.primeiro_nome ilike concat('%',func_nome,'%') 
		or
			funcionarios.ultimo_nome ilike concat('%',func_nome,'%')
	
		group by funcionarios.id_func
		order by funcionarios.primeiro_nome desc;
end; $$


create function pesquisa_horas_mes (mes_x integer, ano_x integer) 
returns table (
	mes double precision,
	horas numeric(4,2)
) language plpgsql
as $$
begin
	return query 
		select
			extract(month from projeto.iniciado),
			sum(projeto_info.horas)

		from projeto_info
			inner join projeto 
				on projeto_info.id_info = projeto.id_info
				
		where 
			extract(month from projeto.iniciado)::integer = mes_x
		and
			extract(year from projeto.iniciado)::integer = ano_x
		
		group by extract(month from projeto.iniciado)
		order by mes_x desc;

end; $$


create function pesquisa_horas_projeto(xnome varchar) 
returns table (
	projeto varchar,
	horas numeric
) language plpgsql
as $$
begin
	return query 
		select
			projeto_info.nome,
			sum(projeto_info.horas)

		from projeto_info
			inner join projeto 
				on projeto_info.id_info = projeto.id_info
		
		where projeto_info.nome ilike concat('%',xnome,'%')
				
		group by projeto_info.nome
		order by projeto_info.nome desc;
end; $$


create function pesquisa_nome_projeto (nome_x varchar) 
returns table (
	id varchar,
	id_func varchar,
	nome varchar,
	decr varchar,
	iniciado timestamp,
	status varchar,
	finalizado integer
) language plpgsql
as $$
begin
	return query 
		select
			projeto.id as id, 
			projeto.id_func as id_func, 
			projeto_info.nome as nome,
			projeto_info.descr as descr,
			projeto.iniciado as iniciado,
			projeto.status as status,
			projeto.finalizado as finalizado
		from projeto 
			inner join projeto_info
				on projeto.id_info = projeto_info.id_info
		where 
			projeto_info.nome ilike concat('%',nome_x,'%')
		order by nome desc;

end; $$


create function pesquisa_quant_status () 
returns table (
	status varchar,
	total bigint
) language plpgsql
as $$
begin
	return query 
		select
			projeto.status,
			count(projeto.status)
		from projeto
		group by projeto.status
		order by projeto.status desc;

end; $$


create function pesquisa_status_projeto (status_x varchar) 
returns table (
	id varchar,
	id_func varchar,
	nome varchar,
	decr varchar,
	iniciado timestamp,
	status varchar,
	finalizado integer
) language plpgsql
as $$
begin
	return query 
		select
			projeto.id, 
			projeto.id_func, 
			projeto_info.nome,
			projeto_info.descr,
			projeto.iniciado,
			projeto.status,
			projeto.finalizado
		from projeto, projeto_info
		where 
			projeto.id_info = projeto_info.id_info
		and
			projeto.status = concat(status_x,'%')
		order by projeto_info.nome desc;

end; $$


create function pesquisa_tasks_abertas() 
returns table (
	id varchar,
	projeto varchar,
	status varchar
) language plpgsql
as $$
begin
	return query
		select
			projeto.id,
			projeto_info.nome,
			projeto.status
			
		from projeto
			inner join projeto_info
				on projeto.id_info = projeto_info.id_info
			
		where
			projeto.finalizado = 0
			
		order by projeto_info.nome desc;
end; $$