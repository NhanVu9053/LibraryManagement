﻿using LM.Domain.Request.Book;
using LM.Domain.Response.Book;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<BookView>> Gets();
        Task<BookView> Get(int id);
        Task<SaveBookRes> Save(SaveBookReq request);
        Task<SaveBookRes> ChangeStatus(StatusBookReq request);
        //void CheckStatusBookIsOver(int id);
    }
}
