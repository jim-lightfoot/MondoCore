/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  	                                            
 *                                                                          
 *      Namespace: MondoCore.Data	                                        
 *           File: IWriteRepository.cs                                                
 *      Class(es): IWriteRepository                                                   
 *        Purpose: Interface to write to a repository                               
 *                                                                          
 *  Original Author: Jim Lightfoot                                         
 *    Creation Date: 24 Feb 2021                                           
 *                                                                          
 *   Copyright (c) 2021 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MondoCore.Data
{
    public interface IWriteRepository<TID>
    {
        Task<object> Insert(TID id, object item);
        Task         Update(TID id, object item);
        Task         Delete(TID id);
    }
}
