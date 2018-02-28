using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;
using VIENNAAddInUtils;
using System.Linq;
using System.Xml;

namespace VIENNAAddIn.upcc3.ea
{
    public class EaUmlRepository : IUmlRepository
    {
        private readonly Repository eaRepository;
        private RepositoryType? _repositoryType;

        public EaUmlRepository(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }
        /// <summary>
	    /// returns the type of repository backend.
	    /// This is mostly needed to adjust to sql to the specific sql dialect
	    /// </summary>
	    public RepositoryType repositoryType
        {
            get
            {
                if (!this._repositoryType.HasValue)
                {
                    this._repositoryType = getRepositoryType();
                }
                return _repositoryType.Value;
            }
        }

        #region IUmlRepository Members

        public IEnumerable<IUmlPackage> GetPackagesByStereotype(params string[] stereotypes)
        {
            var foundPackages = new List<IUmlPackage>();
            foreach (string stereotype in stereotypes)
            {
                string getElementsByStereotypeSQL = formatSQL(@"select o.Object_ID from t_object o
														inner join t_xref x on o.ea_guid = x.Client
														where x.Name = 'Stereotypes' 
														and o.Object_Type = 'Package'
														and x.Description like '%@STEREO;Name=" + stereotype + ";%'");

                foreach (EA.Element element in eaRepository.GetElementSet(getElementsByStereotypeSQL, 2))
                {
                    var package = eaRepository.GetPackageByGuid(element.ElementGUID);
                    if (package != null) foundPackages.Add(new EaUmlPackage(eaRepository, package));
                }
            }
            return foundPackages;
        }

        /// <summary>
        /// returns all packages owned by the package with the vgiven ID or below having one of the p
        /// </summary>
        /// <param name="parentPackageID"></param>
        /// <param name="includeParent"></param>
        /// <param name="stereotypes"></param>
        /// <returns></returns>
        public IEnumerable<IUmlPackage> GetSubPackagesByStereotype(IUmlPackage parentPackage, bool includeParent, params string[] stereotypes)
        {
            var foundPackages = new List<IUmlPackage>();
            //add parentPackage if it has the stereotype
            var packageTreeIDs = parentPackage.getPackageTreeIDs();
            //put each id between single quotes and put commas inbetween e.g. '123','456','789'
            var sqlPackageTreeIDs = string.Join(",", packageTreeIDs.Select(x => "'" + x + "'"));
            foreach (string stereotype in stereotypes)
            {
                //add parent package if needed
                if (includeParent
                    && parentPackage.Stereotypes.Any(x => x == stereotype)
                    && !foundPackages.Any(x => x.Id == parentPackage.Id))
                {
                    foundPackages.Add(parentPackage);
                }
                //search for sub elements
                string getPackageIDsByStereotypeSQL = formatSQL(@"select distinct p.Package_ID from t_object o
														inner join t_xref x on o.ea_guid = x.Client
                                                        inner join t_package p on p.ea_guid = o.ea_guid
														where x.Name = 'Stereotypes' 
														and o.Object_Type = 'Package'
														and x.Description like '%@STEREO;Name=" + stereotype + ";%'" +
                                                        " and o.PDATA1 in (" + sqlPackageTreeIDs + ")");
                XmlDocument results = new XmlDocument();
                results.LoadXml(eaRepository.SQLQuery(getPackageIDsByStereotypeSQL));
                //get the package id's from the query results a
                foreach (XmlNode packageIDNode in results.SelectNodes("//Package_ID"))
                {
                    int packageID;
                    if (int.TryParse(packageIDNode.InnerText, out packageID))
                    {
                        //get the packages
                        var package = eaRepository.GetPackageByID(packageID);
                        if (package != null)
                            foundPackages.Add(new EaUmlPackage(eaRepository, package));
                    }
                }
            }
            return foundPackages;
        }
        public IUmlPackage GetPackageById(int id)
        {
            return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(id));
        }

        public IUmlPackage GetPackageByPath(Path path)
        {
            return new EaUmlPackage(eaRepository, eaRepository.Resolve<Package>(path));
        }

        public IUmlDataType GetDataTypeById(int id)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.GetElementByID(id));
        }

        public IUmlDataType GetDataTypeByPath(Path path)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.Resolve<Element>(path));
        }

        public IUmlEnumeration GetEnumerationById(int id)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.GetElementByID(id));
        }

        public IUmlEnumeration GetEnumerationByPath(Path path)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.Resolve<Element>(path));
        }

        public IUmlClass GetClassById(int id)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.GetElementByID(id));
        }

        public IUmlClass GetClassByPath(Path path)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.Resolve<Element>(path));
        }

        public IEnumerable<Path> GetRootLocations()
        {
            foreach (Package eaModel in eaRepository.Models)
            {
                yield return eaModel.Name;
                foreach (Package rootPackage in eaModel.Packages)
                {
                    if (rootPackage.Element.Stereotype == "bInformationV")
                    {
                        yield return (Path)eaModel.Name / rootPackage.Name;
                    }
                }
            }
        }

        public IUmlPackage CreateRootPackage(Path rootLocation, UmlPackageSpec spec)
        {
            var rootLocationPackage = eaRepository.Resolve<Package>(rootLocation);
            if (rootLocationPackage == null)
            {
                throw new ArgumentException("Invalid root location: " + rootLocation);
            }

            var eaPackage = (Package)rootLocationPackage.Packages.AddNew(spec.Name, string.Empty);
            eaPackage.Update();
            eaPackage.ParentID = rootLocationPackage.PackageID;

            var package = new EaUmlPackage(eaRepository, eaPackage);
            package.Initialize(spec);
            return package;
        }

        #endregion

        private static void EnumeratePackages(Package root, List<Package> packageList)
        {
            packageList.Add(root);
            foreach (Package package in root.Packages)
            {
                EnumeratePackages(package, packageList);
            }
        }
        /// <summary>
	    /// sets the correct wildcards depending on the database type.
	    /// changes '%' into '*' if on ms access
	    /// and _ into ? on msAccess
	    /// </summary>
	    /// <param name="sqlQuery">the original query</param>
	    /// <returns>the fixed query</returns>
	    private string formatSQL(string sqlQuery)
        {
            sqlQuery = replaceSQLWildCards(sqlQuery);
            sqlQuery = formatSQLTop(sqlQuery);
            sqlQuery = formatSQLFunctions(sqlQuery);
            return sqlQuery;
        }

        /// <summary>
        /// Operation to translate SQL functions in there equivalents in different sql syntaxes
        /// supported functions:
        /// 
        /// - lcase -> lower in T-SQL (SQLSVR and ASA)
        /// </summary>
        /// <param name="sqlQuery">the query to format</param>
        /// <returns>a query with traslated functions</returns>
        private string formatSQLFunctions(string sqlQuery)
        {
            string formattedSQL = sqlQuery;
            //lcase -> lower in T-SQL (SQLSVR and ASA and Oracle and FireBird)
            if (this.repositoryType == RepositoryType.SQLSVR ||
                this.repositoryType == RepositoryType.ASA ||
                   this.repositoryType == RepositoryType.ORACLE ||
                   this.repositoryType == RepositoryType.FIREBIRD ||
                   this.repositoryType == RepositoryType.POSTGRES)
            {
                formattedSQL = formattedSQL.Replace("lcase(", "lower(");
            }
            return formattedSQL;
        }

        /// <summary>
        /// limiting the number of results in an sql query is different on different platforms.
        /// 
        /// "SELECT TOP N" is used on
        /// SQLSVR
        /// ADOJET
        /// ASA
        /// OPENEDGE
        /// ACCESS2007
        /// 
        /// "WHERE rowcount <= N" is used on
        /// ORACLE
        /// 
        /// "LIMIT N" is used on
        /// MYSQL
        /// POSTGRES
        /// 
        /// This operation will replace the SELECT TOP N by the appropriate sql syntax depending on the repositorytype
        /// </summary>
        /// <param name="sqlQuery">the sql query to format</param>
        /// <returns>the formatted sql query </returns>
        private string formatSQLTop(string sqlQuery)
        {
            string formattedQuery = sqlQuery;
            string selectTop = "select top ";
            int begintop = sqlQuery.ToLower().IndexOf(selectTop);
            if (begintop >= 0)
            {
                int beginN = begintop + selectTop.Length;
                int endN = sqlQuery.ToLower().IndexOf(" ", beginN) + 1;
                if (endN > beginN)
                {
                    string N = sqlQuery.ToLower().Substring(beginN, endN - beginN);
                    string selectTopN = sqlQuery.Substring(begintop, endN);
                    switch (this.repositoryType)
                    {
                        case RepositoryType.ORACLE:
                            // remove "top N" clause
                            formattedQuery = formattedQuery.Replace(selectTopN, "select ");
                            // find where clause
                            string whereString = "where ";
                            int beginWhere = formattedQuery.ToLower().IndexOf(whereString);
                            string rowcountCondition = "rownum <= " + N + " and ";
                            // add the rowcount condition
                            formattedQuery = formattedQuery.Insert(beginWhere + whereString.Length, rowcountCondition);
                            break;
                        case RepositoryType.MYSQL:
                        case RepositoryType.POSTGRES:
                            // remove "top N" clause
                            formattedQuery = formattedQuery.Replace(selectTopN, "select ");
                            string limitString = " limit " + N;
                            // add limit clause
                            formattedQuery = formattedQuery + limitString;
                            break;
                        case RepositoryType.FIREBIRD:
                            // in firebird top becomes first
                            formattedQuery = formattedQuery.Replace(selectTopN, selectTopN.Replace("top", "first"));
                            break;
                    }
                }
            }
            return formattedQuery;
        }
        /// <summary>
        /// replace the wildcards in the given sql query string to match either MSAccess or ANSI syntax
        /// </summary>
        /// <param name="sqlQuery">the sql string to edit</param>
        /// <returns>the same sql query, but with its wildcards replaced according to the required syntax</returns>
        private string replaceSQLWildCards(string sqlQuery)
        {
            bool msAccess = this.repositoryType == RepositoryType.ADOJET;
            int beginLike = sqlQuery.IndexOf("like", StringComparison.InvariantCultureIgnoreCase);
            if (beginLike > 1)
            {
                int beginString = sqlQuery.IndexOf("'", beginLike + "like".Length);
                if (beginString > 0)
                {
                    int endString = sqlQuery.IndexOf("'", beginString + 1);
                    if (endString > beginString)
                    {
                        string originalLikeString = sqlQuery.Substring(beginString + 1, endString - beginString);
                        string likeString = originalLikeString;
                        if (msAccess)
                        {
                            likeString = likeString.Replace('%', '*');
                            likeString = likeString.Replace('_', '?');
                            likeString = likeString.Replace('^', '!');
                        }
                        else
                        {
                            likeString = likeString.Replace('*', '%');
                            likeString = likeString.Replace('?', '_');
                            likeString = likeString.Replace('#', '_');
                            likeString = likeString.Replace('^', '!');
                        }
                        string next = string.Empty;
                        if (endString < sqlQuery.Length)
                        {
                            next = replaceSQLWildCards(sqlQuery.Substring(endString + 1));
                        }
                        sqlQuery = sqlQuery.Substring(0, beginString + 1) + likeString + next;

                    }
                }
            }
            return sqlQuery;
        }
        /// <summary>
        /// Gets the Repository type for this model
        /// </summary>
        /// <returns></returns>
        public RepositoryType getRepositoryType()
        {
            string connectionString = this.eaRepository.ConnectionString;
            RepositoryType repoType = RepositoryType.ADOJET; //default to .eap file

            // if it is a .feap file then it surely is a firebird db
            if (connectionString.ToLower().EndsWith(".feap"))
            {
                repoType = RepositoryType.FIREBIRD;
            }
            else
            {
                //if it is a .eap file we check the size of it. if less then 1 MB then it is a shortcut file and we have to open it as a text file to find the actual connection string
                if (connectionString.ToLower().EndsWith(".eap"))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(connectionString);
                    if (fileInfo.Length > 1000)
                    {
                        //local .eap file, ms access syntax
                        repoType = RepositoryType.ADOJET;
                    }
                    else
                    {
                        //open the file as a text file to find the connectionstring.
                        System.IO.FileStream fileStream = new System.IO.FileStream(connectionString, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                        System.IO.StreamReader reader = new System.IO.StreamReader(fileStream);
                        //replace connectionstring with the file contents
                        connectionString = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                if (!connectionString.ToLower().EndsWith(".eap"))
                {
                    string dbTypeString = "DBType=";
                    int dbIndex = connectionString.IndexOf(dbTypeString) + dbTypeString.Length;
                    if (dbIndex > dbTypeString.Length)
                    {
                        int dbNumber;
                        string dbNumberString = connectionString.Substring(dbIndex, 1);
                        if (int.TryParse(dbNumberString, out dbNumber))
                        {
                            repoType = (RepositoryType)dbNumber;
                        }
                    }
                }
            }
            return repoType;
        }
    }
    /// <summary>
	/// List of databses supported as backend for an EA repository
	/// 0 - MYSQL
	///	1 - SQLSVR
	/// 2 - ADOJET
	/// 3 - ORACLE
	/// 4 - POSTGRES
	/// 5 - ASA
	/// 7 - OPENEDGE
	/// 8 - ACCESS2007
	/// 9 - FireBird
	/// </summary>
	public enum RepositoryType
    {
        MYSQL,
        SQLSVR,
        ADOJET,
        ORACLE,
        POSTGRES,
        ASA,
        OPENEDGE,
        ACCESS2007,
        FIREBIRD
    }
}