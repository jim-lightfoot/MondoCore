/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  	                                            
 *                                                                          
 *      Namespace: MondoCore.Data	                                        
 *           File: IReadRepository.cs                                                
 *      Class(es): IReadRepository                                                   
 *        Purpose: Interface to query a repository                               
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
    public interface IReadRepository<TID>
    {
        Task<object>              Get(TID id);
        Task<IEnumerable<object>> Get(IEnumerable<TID> ids);
    }
}
