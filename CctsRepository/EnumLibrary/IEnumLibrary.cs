
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
// ReSharper disable RedundantUsingDirective
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
// ReSharper restore RedundantUsingDirective

namespace CctsRepository.EnumLibrary
{
	/// <summary>
	/// Interface for CCTS/UPCC ENUMLibrary.
	/// </summary>
    public partial interface IEnumLibrary
    {
		/// <summary>
		/// The ENUMLibrary's unique ID.
		/// </summary>
        int Id { get; }
		
		/// <summary>
		/// The ENUMLibrary's name.
		/// </summary>
        string Name { get; }

		/// <summary>
		/// The bLibrary containing this ENUMLibrary.
		/// </summary>
		IBLibrary BLibrary { get; }

		/// <summary>
		/// The ENUMs contained in this ENUMLibrary.
		/// </summary>
		IEnumerable<IEnum> Enums { get; }

		/// <summary>
		/// Retrieves a ENUM by name.
		/// <param name="name">A ENUM's name.</param>
		/// <returns>The ENUM with the given <paramref name="name"/> or <c>null</c> if no such ENUM is found.</returns>
		/// </summary>
        IEnum GetEnumByName(string name);

		/// <summary>
		/// Creates a ENUM based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ENUM.</param>
		/// <returns>The newly created ENUM.</returns>
		/// </summary>
		IEnum CreateEnum(EnumSpec specification);

		/// <summary>
		/// Updates a ENUM to match the given <paramref name="specification"/>.
		/// <param name="@enum">A ENUM.</param>
		/// <param name="specification">A new specification for the given ENUM.</param>
		/// <returns>The updated ENUM. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IEnum UpdateEnum(IEnum @enum, EnumSpec specification);

		/// <summary>
		/// Removes a ENUM from this ENUMLibrary.
		/// <param name="@enum">A ENUM.</param>
		/// </summary>
        void RemoveEnum(IEnum @enum);

		/// <summary>
		/// The IDSCHEMEs contained in this ENUMLibrary.
		/// </summary>
		IEnumerable<IIdScheme> IdSchemes { get; }

		/// <summary>
		/// Retrieves a IDSCHEME by name.
		/// <param name="name">A IDSCHEME's name.</param>
		/// <returns>The IDSCHEME with the given <paramref name="name"/> or <c>null</c> if no such IDSCHEME is found.</returns>
		/// </summary>
        IIdScheme GetIdSchemeByName(string name);

		/// <summary>
		/// Creates a IDSCHEME based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a IDSCHEME.</param>
		/// <returns>The newly created IDSCHEME.</returns>
		/// </summary>
		IIdScheme CreateIdScheme(IdSchemeSpec specification);

		/// <summary>
		/// Updates a IDSCHEME to match the given <paramref name="specification"/>.
		/// <param name="idScheme">A IDSCHEME.</param>
		/// <param name="specification">A new specification for the given IDSCHEME.</param>
		/// <returns>The updated IDSCHEME. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IIdScheme UpdateIdScheme(IIdScheme idScheme, IdSchemeSpec specification);

		/// <summary>
		/// Removes a IDSCHEME from this ENUMLibrary.
		/// <param name="idScheme">A IDSCHEME.</param>
		/// </summary>
        void RemoveIdScheme(IIdScheme idScheme);

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		IEnumerable<string> BusinessTerms { get; }

        ///<summary>
        /// Tagged value 'copyright'.
        ///</summary>
		IEnumerable<string> Copyrights { get; }

        ///<summary>
        /// Tagged value 'owner'.
        ///</summary>
		IEnumerable<string> Owners { get; }

        ///<summary>
        /// Tagged value 'reference'.
        ///</summary>
		IEnumerable<string> References { get; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		string Status { get; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		string UniqueIdentifier { get; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		string VersionIdentifier { get; }

        ///<summary>
        /// Tagged value 'baseURN'.
        ///</summary>
		string BaseURN { get; }

        ///<summary>
        /// Tagged value 'namespacePrefix'.
        ///</summary>
		string NamespacePrefix { get; }
    }
}

