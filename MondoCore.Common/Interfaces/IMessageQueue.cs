/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  	                                             
 *                                                                           
 *      Namespace: MondoCore.Common	                                         
 *           File: IMessageQueue.cs                                             
 *      Class(es): IMessageQueue                                                
 *        Purpose: Generic interface for sending messages                      
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 3 Dec 2017                                             
 *                                                                           
 *   Copyright (c) 2015-2020 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// Interface for sending messages
    /// </summary>
    public interface IMessageQueue
    {
        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="sendOn">Optional time when to send</param>
        Task Send(string message, DateTimeOffset? sendOn = null);

        /// <summary>
        /// Retrieve a single message from the queue
        /// </summary>
        Task<IMessage> Retrieve();

        /// <summary>
        /// Delete a message from the queu
        /// </summary>
        /// <param name="message">Message to delete</param>
        Task Delete(IMessage message);
    }

    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// Interface for a message
    /// </summary>
    public interface IMessage
    {
         string Value  { get; }
    }

    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// Interface for for sending messages
    /// </summary>
    public interface IMessageQueueFactory
    {
        /// <summary>
        /// Create a message queue
        /// </summary>
        /// <param name="queueName">Name of message queue to create</param>
        IMessageQueue CreateQueue(string queueName);
    }
}
