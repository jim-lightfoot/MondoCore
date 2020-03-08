/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  	                                             
 *                                                                           
 *      Namespace: MondoCore.Common	                                         
 *           File: ICache.cs                                                 
 *      Class(es): ICache, ICacheDependency, FileDependency                  
 *        Purpose: Generic cache interface                                   
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 29 Nov 2015                                             
 *                                                                           
 *   Copyright (c) 2015-2020 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public interface ICache
    {
        Task<object>  Get(string key);

        Task          Add(string key, object objToAdd);
        Task          Add(string key, object objToAdd, DateTime dtExpires, ICacheDependency dependency = null);
        Task          Add(string key, object objToAdd, TimeSpan tsExpires, ICacheDependency dependency = null);
                      
        Task          Remove(string key);
    }

    /****************************************************************************/
    /****************************************************************************/
    public interface ICacheDependency
    {
        string Type { get; }
    }

    /****************************************************************************/
    /****************************************************************************/
    public class FileDependency : ICacheDependency
    {
        /****************************************************************************/
        public FileDependency(string fileName)
        {
            this.FileName = fileName;
        }

        /****************************************************************************/
        public string Type      => "file";
        public string FileName  { get; }
    }
}
