create database "NoteSystem"
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
    avatar varchar(100) default 'http://placeimg.com/210/210/people',
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
	refFunc varchar := func->>'id_func';
begin
	case when 
		(select (count(id_func)::int)::boolean from funcionarios where id_func like refFunc)
	then
		update funcionarios
		set
			primeiro_nome = func->>'primeiro_nome',
			ultimo_nome = func->>'ultimo_nome',
			avatar = func->>'avatar',
			email = func->>'email'
		where
			funcionarios.id_func = refFunc;
		
	else
		insert into funcionarios values
		(
			refFunc,
			func->>'primeiro_nome',
			func->>'ultimo_nome',
			func->>'avatar',
			func->>'email'
		);
		
	end case;
	
	return (select (count(id_func)::int)::boolean from funcionarios where id_func like refFunc);
end $$;


create or replace function inserir_projInfo(projInfo json)
returns boolean
language plpgsql
as $$
declare
	refNomeInfo varchar := projInfo->>'nome';
	refDescrInfo varchar := projInfo->>'descr';
	refHoras numeric(4,2) := cast(projInfo->>'horas' as numeric(4,2));
begin
	case when
		(
			select (count(descr)::int)::boolean 
				from projeto_info 
			where 
				nome like refNomeInfo 
			and
				descr like refDescrInfo
		)
	then
		update projeto_info
		set
			horas = refHoras
		where
			nome like refNomeInfo 
		and
			descr like refDescrInfo;
		
	else 
		insert into projeto_info(nome,descr,horas) values
		(
			refNomeInfo,
			refDescrInfo,
			refHoras
		);
	end case;
	
	return (
		select (count(descr)::int)::boolean 
			from projeto_info 
		where 
			nome like refNomeInfo 
		and
			descr like refDescrInfo
	);
end $$;


create or replace function inserir_git(git json)
returns boolean
language plpgsql
as $$
declare
	refGit varchar := git->>'hash';
begin
	case when 
		(select (count(hash)::int)::boolean from gitmetadata where hash like refGit) = false
	then
		insert into gitmetadata(branch,hash) values
		(
			git->>'branch',
			refGit
		);
		
	else end case;
	
	return (select (count(hash)::int)::boolean from gitmetadata where hash like refGit) = false;
end $$;


create or replace function inserir_projeto(proj json)
returns boolean
language plpgsql
as $$
declare
	refProj varchar := proj->>'id';
	refFunc varchar := proj->>'id_func';
	refInfo int := (
		select id_info from projeto_info
		where nome like proj->>'nome_info'
		and descr like proj->>'descr_info'
	);
	refGit int  := (select id_git from gitmetadata where hash like (proj->>'hash_git'));
begin
	case when 
		(select (count(id)::int)::boolean from projeto where id like refProj)
	then
		update projeto
		set
			id_func = refFunc,
			id_info = refInfo,
			id_git = refGit,
			iniciado = to_timestamp(proj->>'iniciado', 'MM/DD/YYYY HH24:MI:SS'),
			status = proj->>'status',
			finalizado = ((proj->>'finalizado')::boolean)::integer
		where
			id = refProj;
		
	else
		insert into projeto values
		(
			refProj,
			refFunc,
			refInfo,
			refGit,
			to_timestamp(proj->>'iniciado', 'MM/DD/YYYY HH24:MI:SS'),
			proj->>'status',
			((proj->>'finalizado')::boolean)::integer
		);
		
	end case;
	
	return (select (count(id)::int)::boolean from projeto where id like refProj);
end $$;


create or replace function pesquisa_dedicacao_func (nome_projeto varchar) 
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


create or replace function pesquisa_finalizado_projeto (finalizado_x integer) 
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


create or replace function pesquisa_horas_ano (ano_x integer) 
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


create or replace function pesquisa_horas_funcionario (func_nome varchar) 
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


create or replace function pesquisa_horas_mes (mes_x integer, ano_x integer) 
returns table (
	mes double precision,
	horas numeric(4,2)
) language plpgsql
as $$
begin
	return query 
		select
			extract(month from projeto.iniciado) as mes,
			extract(year from projeto.iniciado) as ano,
			sum(projeto_info.horas)

		from projeto_info
			inner join projeto 
				on projeto_info.id_info = projeto.id_info

		where case
			when mes_x != 0
			then
				extract(month from projeto.iniciado)::integer = mes_x
				and
				extract(year from projeto.iniciado)::integer = ano_x
			
			else end case;

		group by mes, extract(year from projeto.iniciado)
		order by mes desc;

end; $$


create or replace function pesquisa_horas_projeto(xnome varchar) 
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


create or replace function pesquisa_nome_projeto (nome_x varchar) 
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
			projeto.iniciado as a,
			projeto.status,
			projeto.finalizado
		from projeto 
			inner join projeto_info
				on projeto.id_info = projeto_info.id_info
		where 
			projeto_info.nome ilike concat('%',nome_x,'%')
		order by a desc;

end; $$


create or replace function pesquisa_quant_status () 
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


create or replace function pesquisa_status_projeto (status_x varchar) 
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


create or replace function pesquisa_tasks_abertas() 
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
			
		order by projeto.iniciado asc
		limit 5;
end; $$