/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Security.Encryption				            
 *             File: EncryptionPolicy.cs					    		    
 *        Class(es): EncryptionPolicy				         		        
 *          Purpose: An encryption policy is used to specify an encr. key   
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 19 Jan 2020                                            
 *                                                                          
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// An encryption policy is used to specify an encryption key. The policy id is the same as the key id. 
    /// Once created or loaded it cannot be modified.
    /// </summary>
    public class EncryptionPolicy
    {
        private Guid     _id          = Guid.NewGuid();
        private string   _algorithm   = "AES";
        private int      _keySize     = 256;
        private int?     _blockSize   = 128; 
        private string   _padding     = null; // Use default for algorithm
        private string   _mode        = null; // Use default for algorithm 
        private DateTime _expires     = DateTime.UtcNow.AddMonths(3);

        public Guid     Id            { get { return _id; }        set { if(this.IsReadOnly) throw new ReadOnlyException(); _id        = value; } }
        public string   Algorithm     { get { return _algorithm; } set { if(this.IsReadOnly) throw new ReadOnlyException(); _algorithm = value; } }
        public int      KeySize       { get { return _keySize; }   set { if(this.IsReadOnly) throw new ReadOnlyException(); _keySize   = value; } }
        public int?     BlockSize     { get { return _blockSize; } set { if(this.IsReadOnly) throw new ReadOnlyException(); _blockSize = value; } }
        public string   Padding       { get { return _padding; }   set { if(this.IsReadOnly) throw new ReadOnlyException(); _padding   = value; } }
        public string   Mode          { get { return _mode; }      set { if(this.IsReadOnly) throw new ReadOnlyException(); _mode      = value; } }
        public DateTime Expires       { get { return _expires; }   set { if(this.IsReadOnly) throw new ReadOnlyException(); _expires   = value; } } 

        public bool     IsExpired     => this.Expires < DateTime.UtcNow;
        public bool     IsReadOnly    { get; set; } = false;

        /****************************************************************************/
        public class ReadOnlyException : Exception
        {
            public ReadOnlyException() : base("Once a policy has been initialized in cannot be modified")
            {
            }
        }

        /****************************************************************************/
        /// <summary>
        /// Return new policy but with new id and reset expirate date
        /// </summary>
        /// <returns>Cloned policy</returns>
        public EncryptionPolicy Clone(TimeSpan expires)
        {
            return new EncryptionPolicy
            { 
                Algorithm  = this.Algorithm,
                KeySize    = this.KeySize,  
                BlockSize  = this.BlockSize,
                Padding    = this.Padding,  
                Mode       = this.Mode,  
                Expires    = DateTime.UtcNow + expires
            };
        }
    }
}
