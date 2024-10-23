/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  	                                            
 *                                                                          
 *      Namespace: MondoCore.Data	                                        
 *           File: IDatabase.cs                                                
 *      Class(es): IDatabase                                                   
 *        Purpose: Interface to query a database                               
 *                                                                          
 *  Original Author: Jim Lightfoot                                         
 *    Creation Date: 24 Feb 2021                                           
 *                                                                          
 *   Copyright (c) 2021-2024 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

namespace MondoCore.Data
{
    /// <summary>
    /// Interface for a database
    /// </summary>
    public interface IDatabase
    {
        IReadRepository<TID, TValue> GetRepositoryReader<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID>;
        IWriteRepository<TID, TValue> GetRepositoryWriter<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID>;
    }
}
