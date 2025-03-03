using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;

namespace eRekreacija.Services.Services
{
    public class AppointmentService:BaseCRUDService<tbl_Appointment,AppointmentDTO,AppointmentInsertRequest,object>,IAppointmentService
    {
        public AppointmentService(RekreacijaContext rekreacijaContext,IMapper mapper):base(rekreacijaContext,mapper){}

        public override async Task BeforeInsert(tbl_Appointment db, AppointmentInsertRequest insert)
        {
            var payment = new tbl_Payment
            {
                appointment_id=db.id,
                object_id=db.object_id,
                paid_date=db.appointment_date,
                user_id=insert.user_id,
                amount=insert.amount
            };

            await _rekreacijaContext.AddAsync(payment);
            await _rekreacijaContext.SaveChangesAsync();
            await base.BeforeInsert(db, insert);
        }
    }
}