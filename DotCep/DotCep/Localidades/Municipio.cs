﻿using System;
using System.Data;
using System.Collections.Generic;

namespace DotCEP.Localidades
{
	public class Municipio
	{
		#region Propriedades
		public int CodigoEstado { get; set; }
		public int Codigo { get; set; }
		public string Nome { get; set; }
		#endregion

		public static Municipio ObterInformacoesDoMunicipio(string NomeMunicipio, UF SiglaEstado)
		{
			Municipio municipioBase = new Municipio();
			Spartacus.Database.Command cmd = new Spartacus.Database.Command();
			DataTable tabelaResultado = new DataTable();

			cmd.v_text = "Select * from Municipios m where m.nome = #nome# and m.codigoestado = #codigo#";
			cmd.AddParameter("nome", Spartacus.Database.Type.STRING);
			cmd.AddParameter("codigo", Spartacus.Database.Type.INTEGER);

			cmd.SetValue("nome", NomeMunicipio);
			cmd.SetValue("codigo", Convert.ToInt16(DotCEP.UF.RS).ToString());

			tabelaResultado = ObterInformacoesDoBanco(cmd.GetUpdatedText());

			if (tabelaResultado.Rows.Count != 0)
			{
				municipioBase = ConverterRowParaEntidade(tabelaResultado.Rows[0]);
			}


			return municipioBase;
		}

		public static Municipio ObterInformacoesDoMunicipio(uint CodigoMunicipio)
		{
			Municipio municipioBase = new Municipio();
			Spartacus.Database.Command cmd = new Spartacus.Database.Command();
			DataTable tabelaResultado = new DataTable();

			cmd.v_text = "Select * from Municipios m where m.Codigo = #codigo#";
			cmd.AddParameter("codigo", Spartacus.Database.Type.INTEGER);
			cmd.SetValue("codigo", CodigoMunicipio.ToString());

			tabelaResultado = ObterInformacoesDoBanco(cmd.GetUpdatedText());

			if (tabelaResultado.Rows.Count != 0)
			{
				municipioBase = ConverterRowParaEntidade(tabelaResultado.Rows[0]);
			}

			return municipioBase;
		}

		public static String ObterNomeDoMunicipio(uint CodigoMunicipio)
		{
			String saida = String.Empty;
			Spartacus.Database.Command cmd = new Spartacus.Database.Command();
			DataTable tabelaResultado;

			cmd.v_text = "select t.nome from Municipios t where t.codigo = #codigo#";
			cmd.AddParameter("codigo", Spartacus.Database.Type.INTEGER);
			cmd.SetValue("codigo", CodigoMunicipio.ToString());

			tabelaResultado = ObterInformacoesDoBanco(cmd.GetUpdatedText());

			if (tabelaResultado.Rows.Count != 0)
			{
				saida = tabelaResultado.Rows[0]["nome"].ToString();
			}

			return saida;
		}

		public static int ObterCodigoDoMunicipio(string NomeMunicipio, UF SiglaEstado)
		{
			int saida = 0;
			Spartacus.Database.Command cmd = new Spartacus.Database.Command();
			DataTable tabelaResultado;

			cmd.v_text = "select t.Codigo from Municipios t where t.nome = #nome# and t.CodigoEstado = #estado#";
			cmd.AddParameter("nome", Spartacus.Database.Type.STRING);
			cmd.AddParameter("estado", Spartacus.Database.Type.INTEGER);

			cmd.SetValue("nome", NomeMunicipio);
			cmd.SetValue("estado", Convert.ToInt16(DotCEP.UF.RS).ToString());

			tabelaResultado = ObterInformacoesDoBanco(cmd.GetUpdatedText());

			if (tabelaResultado.Rows.Count != 0)
			{
				saida = Convert.ToInt32(tabelaResultado.Rows[0]["Codigo"].ToString());
			}

			return saida;
		}

		public static List<Municipio> ObterListaDeMunicipio()
		{
			List<Municipio> listaDeMunicipios = new List<Municipio>();
			DataTable tabelaResultado;
			Spartacus.Database.Command cmd = new Spartacus.Database.Command();

			cmd.v_text = "select t.* from Municipios t";
			tabelaResultado = ObterInformacoesDoBanco(cmd.GetUpdatedText());

			foreach (DataRow item in tabelaResultado.Rows)
			{
				listaDeMunicipios.Add(ConverterRowParaEntidade(item));
			}

			return listaDeMunicipios;
		}

		public static List<Municipio> ObterListaDeMunicipio(UF SiglaEstado)
		{
			List<Municipio> listaDeMunicipios = new List<Municipio>();
			DataTable tabelaResultado;
			Spartacus.Database.Command cmd = new Spartacus.Database.Command();

			cmd.v_text = "select t.* from Municipios t where t.CodigoEstado = #codigo#";
			cmd.AddParameter("codigo", Spartacus.Database.Type.INTEGER);
			cmd.SetValue("codigo", Convert.ToInt16(DotCEP.UF.RS).ToString());

			tabelaResultado = ObterInformacoesDoBanco(cmd.GetUpdatedText());

			foreach (DataRow item in tabelaResultado.Rows)
			{
				listaDeMunicipios.Add(ConverterRowParaEntidade(item));
			}

			return listaDeMunicipios;
		}

		private static Municipio ConverterRowParaEntidade(DataRow LinhaDaTabela)
		{
			Municipio municipioBase = new Municipio();

			municipioBase.Codigo = Convert.ToInt32(LinhaDaTabela["Codigo"]);
			municipioBase.CodigoEstado = Convert.ToInt16(LinhaDaTabela["CodigoEstado"]);
			municipioBase.Nome = LinhaDaTabela["Nome"].ToString();

			return municipioBase;
		}

		private static DataTable ObterInformacoesDoBanco(string p_Query)
		{
			DataTable tabelaSaida = new DataTable();
			Spartacus.Database.Generic database;
			try
			{
				database = new Spartacus.Database.Sqlite(Ferramentas.ObterCaminhoBancoLugare());
				tabelaSaida = database.Query(p_Query, "Resultado");
			}
			catch (Spartacus.Database.Exception ex)
			{
				throw new Exception(ex.v_message);
			}

			return tabelaSaida;
		}
	}
}