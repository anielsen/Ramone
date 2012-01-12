﻿using System;
using System.IO;
using System.Xml;
using Ramone;


namespace Ramone.MediaTypes
{
  /// <summary>
  /// Base for a type-checked codec which will only serialize objects of type TEntity
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  public abstract class XmlCodecBase<TEntity> 
    : IMediaTypeWriter, IMediaTypeReader where TEntity : class      
  {
    #region Ramone.IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      using (var reader = XmlReader.Create(context.HttpStream))
      {
        return ReadFrom(reader);
      }
    }

    #endregion


    #region IMediaTypeWriter

    public void WriteTo(WriterContext context)
    {
      using (var writer = XmlWriter.Create(context.HttpStream))
      {
        WriteTo(context.Data as TEntity, writer);
      }
    }

    #endregion


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion


    protected abstract TEntity ReadFrom(XmlReader reader);

    protected abstract void WriteTo(TEntity item, XmlWriter writer);
  }
}