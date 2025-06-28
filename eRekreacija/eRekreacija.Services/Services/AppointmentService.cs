using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace eRekreacija.Services.Services
{
    public class AppointmentService : BaseCRUDService<tbl_Appointment, AppointmentDTO, AppointmentInsertRequest, object>, IAppointmentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IModel _channel;
        private readonly string _host = Environment.GetEnvironmentVariable("RabbitMQ_Host") ?? "localhost";
        private readonly string _username = Environment.GetEnvironmentVariable("RabbitMQ_Username") ?? "guest";
        private readonly string _password = Environment.GetEnvironmentVariable("RabbitMQ_Password") ?? "guest";
        private readonly string _virtualhost = Environment.GetEnvironmentVariable("RabbitMQ_Virtualhost") ?? "/";

        public AppointmentService(RekreacijaContext rekreacijaContext, IMapper mapper, UserManager<ApplicationUser> userManager) : base(rekreacijaContext, mapper)
        {
            _userManager = userManager;

            var factory = new ConnectionFactory
            {
                HostName = _host,
                UserName = _username,
                Password = _password
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "registrationQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public override async Task<AppointmentDTO> Insert(AppointmentInsertRequest model)
        {
            var entity = _mapper.Map<tbl_Appointment>(model);
            await _rekreacijaContext.Set<tbl_Appointment>().AddAsync(entity);
            await _rekreacijaContext.SaveChangesAsync();

            await BeforeInsert(entity, model);

            var user = await _userManager.FindByIdAsync(model.user_id);

            string message = $"Dear {user.FirstName} {user.LastName},\n\n" +
                                   $"Thank you for reaching out with your appointment request for {model.appointment_date.Value.ToShortDateString()}. We have received your request and will review our schedule to confirm availability\n" +
                                   $"\n\n I will get back to you shortly with a confirmation.\n\n" +
                                   $"\n\n Looking forward to coordinating with you.\n\n" +
                                   $"Best regards Your rekreacija Team.";

            var emailToSent = new
            {
                Email = user.Email,
                Message = message,
                Subject = "Appointment Request Received"
            };

            var meesageJson = JsonConvert.SerializeObject(emailToSent);
            var body = Encoding.UTF8.GetBytes(meesageJson);

            _channel.BasicPublish(exchange: "", routingKey: "registrationQueue", basicProperties: null, body: body);

            return _mapper.Map<AppointmentDTO>(entity);
        }

        public override async Task BeforeInsert(tbl_Appointment db, AppointmentInsertRequest insert)
        {
            var payment = new tbl_Payment
            {
                appointment_id = db.id,
                object_id = db.object_id,
                paid_date = db.appointment_date,
                user_id = insert.user_id,
                amount = insert.amount
            };

            await _rekreacijaContext.AddAsync(payment);
            await _rekreacijaContext.SaveChangesAsync();
            await base.BeforeInsert(db, insert);
        }

        public async Task<List<AppointmentDTO>> GetAppointmentOfObject(string userId)
        {
            var objectIds = await _rekreacijaContext.Set<tbl_Objects>().Where(s => s.user_id == userId).Select(s => s.id).ToListAsync();

            var appointments = await _rekreacijaContext.Set<tbl_Appointment>().Where(s => objectIds.Contains(s.object_id) && s.is_approved == false)
                .Select(s => new AppointmentDTO
                {
                    id = s.id,
                    appointment_date = s.appointment_date,
                    start_time = s.start_time,
                    end_time = s.end_time,
                    object_id = s.object_id,
                    user_id = s.user_id
                }).ToListAsync();

            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(o => objectIds.Contains(o.id))
            .Select(o => new ObjectsDTO
            {
                id = o.id,
                name = o.name
            }).ToListAsync();

            var obj = objects.ToDictionary(o => o.id, o => o);

            var userIds = appointments.Select(a => a.user_id).Distinct().ToList();
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id))
          .Select(u => new ApplicationUserDTO
          {
              Id = u.Id,
              FirstName = u.FirstName,
              LastName = u.LastName,
          }).ToListAsync();
            var userDict = users.ToDictionary(u => u.Id, u => u);

            foreach (var appointment in appointments)
            {
                if (obj.TryGetValue(appointment.object_id, out var objectsDTO))
                    appointment.object_name = objectsDTO.name;

                if (userDict.TryGetValue(appointment.user_id, out var userDTO))
                    appointment.fullname = userDTO.FirstName + ' ' + userDTO.LastName;
            }

            return appointments;
        }

        public async Task<List<AppointmentDTO>> GetApprovedAppointmentOfObject(string userId)
        {
            var objectIds = await _rekreacijaContext.Set<tbl_Objects>().Where(s => s.user_id == userId).Select(s => s.id).ToListAsync();

            var appointments = await _rekreacijaContext.Set<tbl_Appointment>().Where(s => objectIds.Contains(s.object_id) && s.is_approved == true)
                .Select(s => new AppointmentDTO
                {
                    id = s.id,
                    appointment_date = s.appointment_date,
                    start_time = s.start_time,
                    end_time = s.end_time,
                    object_id = s.object_id,
                    user_id = s.user_id,
                    is_approved = s.is_approved
                }).ToListAsync();

            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(o => objectIds.Contains(o.id))
            .Select(o => new ObjectsDTO
            {
                id = o.id,
                name = o.name
            }).ToListAsync();

            var obj = objects.ToDictionary(o => o.id, o => o);

            var userIds = appointments.Select(a => a.user_id).Distinct().ToList();
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id))
          .Select(u => new ApplicationUserDTO
          {
              Id = u.Id,
              FirstName = u.FirstName,
              LastName = u.LastName,
          }).ToListAsync();
            var userDict = users.ToDictionary(u => u.Id, u => u);

            foreach (var appointment in appointments)
            {
                if (obj.TryGetValue(appointment.object_id, out var objectsDTO))
                    appointment.object_name = objectsDTO.name;

                if (userDict.TryGetValue(appointment.user_id, out var userDTO))
                    appointment.fullname = userDTO.FirstName + ' ' + userDTO.LastName;
            }

            return appointments;
        }

        public async Task<bool> ApproveAppointment(int id)
        {
            var existing_appointment = await _rekreacijaContext.Set<tbl_Appointment>().Where(a => a.id == id).FirstOrDefaultAsync();
            if (existing_appointment != null)
            {
                existing_appointment.is_approved = true;
                await _rekreacijaContext.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(existing_appointment.user_id);

                string message = $"Hi {user.FirstName} {user.LastName},\n" +
                                       $"I am writting to confirm that your appointment for {existing_appointment.appointment_date.Value.ToShortDateString()} has been approved.\n" +
                                       $"\n\n Loking forward to connecting with you.\n\n" +
                                       $"Best regards Your rekreacija Team.";

                var emailToSent = new
                {
                    Email = user.Email,
                    Message = message,
                    Subject = "Your Appointment Has Been Approved"
                };

                var meesageJson = JsonConvert.SerializeObject(emailToSent);
                var body = Encoding.UTF8.GetBytes(meesageJson);

                _channel.BasicPublish(exchange: "", routingKey: "registrationQueue", basicProperties: null, body: body);

                return true;
            }
            else
            {
                return false;
            }

        }

        public override async Task<bool> Delete(int id)
        {
            var existing_appointment = await _rekreacijaContext.Set<tbl_Appointment>().Where(a => a.id == id).FirstOrDefaultAsync();
            if (existing_appointment != null)
            {
                _rekreacijaContext.Remove(existing_appointment);
                await _rekreacijaContext.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(existing_appointment.user_id);

                string message = $"Dear {user.FirstName} {user.LastName},\n" +
                    $"Thank you for your request regarding an appointment. Unfortunately, I am unable to approve the proposed metting on {existing_appointment.appointment_date.Value.ToShortDateString()}\n\n" +
                    $"I sincerely apologize for any inconvenience this may cause. If you would like to propose an alternative date and time, I will do my best to acommodate your request\n\n" +
                     $"Best regards Your rekreacija Team.";

                var emailToSent = new
                {
                    Email = user.Email,
                    Message = message,
                    Subject = "Appointment Request Update"
                };

                var meesageJson = JsonConvert.SerializeObject(emailToSent);
                var body = Encoding.UTF8.GetBytes(meesageJson);

                _channel.BasicPublish(exchange: "", routingKey: "registrationQueue", basicProperties: null, body: body);


                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<MyClientsDTO>> GetMyClients(string userId)
        {
            var objectsIds = await _rekreacijaContext.Set<tbl_Objects>().Where(s => s.user_id == userId).Select(s => s.id).ToListAsync();

            var appointmnetsUser = _rekreacijaContext.Set<tbl_Appointment>().Where(s => objectsIds.Contains(s.object_id) && s.is_approved == true).ToList();

            var appointmentStats = appointmnetsUser.GroupBy(a => a.user_id).ToDictionary(g => g.Key, g => new
            {
                NumberOfAppointments = g.Count(),
                LastAppointmentDate = g.Max(x => x.appointment_date),
            });

            var userIds = appointmentStats.Keys.ToList();

            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).Select(u => new ApplicationUserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
            }).ToListAsync();

            var myClients = users.Select(u => new MyClientsDTO
            {
                FullName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                NumberOfAppointments = appointmentStats[u.Id].NumberOfAppointments,
                LastAppointmentDate = appointmentStats[u.Id].LastAppointmentDate,
            }).ToList();

            return myClients;

        }

        public async Task<List<MyClientPayments>> GetMyClientPayments(string userId)
        {
            var objectsIds = await _rekreacijaContext.Set<tbl_Objects>().Where(s => s.user_id == userId).Select(s => s.id).ToListAsync();

            var appointmnetsIds = _rekreacijaContext.Set<tbl_Appointment>().Where(s => objectsIds.Contains(s.object_id) && s.is_approved == true).Select(s => s.id).ToList();

            var payments = await _rekreacijaContext.Set<tbl_Payment>().Where(s => appointmnetsIds.Contains(s.appointment_id))
                .Select(s => new MyClientPayments
                {
                    Amount = s.amount,
                    AppointmentDate = s.paid_date,
                    user_id = s.user_id,
                    object_id = s.object_id,
                })
                .ToListAsync();

            var objectIds = payments.Select(s => s.object_id).Distinct().ToList();
            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(s => objectIds.Contains(s.id)).Select(s => new
            {
                Id = s.id,
                ObjectName = s.name
            }).ToListAsync();
            var obj = objects.ToDictionary(o => o.Id, o => o);

            var userIds = payments.Select(s => s.user_id).Distinct().ToList();
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).Select(u => new ApplicationUserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
            }).ToListAsync();
            var userDict = users.ToDictionary(u => u.Id, u => u);


            foreach (var p in payments)
            {
                if (obj.TryGetValue(p.object_id, out var objectDTO))
                    p.ObjectName = objectDTO.ObjectName;

                if (userDict.TryGetValue(p.user_id, out var userDTO))
                {
                    p.FullName = userDTO.FirstName + ' ' + userDTO.LastName;
                    p.Email = userDTO.Email;
                    p.Phone = userDTO.PhoneNumber;
                }
            }

            return payments;
        }

        public async Task<List<MyReservationDTO>> GetMyReservation(string userId)
        {
            var reservation = await _rekreacijaContext.Set<tbl_Appointment>().Where(s => s.user_id == userId && s.appointment_date >= DateTime.Now).Select(s => new MyReservationDTO
            {
                AppointmentDate = s.appointment_date,
                AppointmentStartDate = s.start_time,
                AppointmentEndDate = s.end_time,
                object_id = s.object_id,
                is_approved = s.is_approved,
                number_of_players = s.number_of_players,
                price = s.TblPayment.amount
            }).ToListAsync();

            var objectIds = reservation.Select(s => s.object_id).Distinct().ToList();
            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(s => objectIds.Contains(s.id)).Select(s => new
            {
                Id = s.id,
                ObjectName = s.name,
                ObjectAdress = s.address,
                ObjectImage = s.ImagePath
            }).ToListAsync();
            var obj = objects.ToDictionary(o => o.Id, o => o);

            foreach (var r in reservation)
            {
                if (obj.TryGetValue(r.object_id, out var o))
                {
                    r.ObjectAdress = o.ObjectAdress;
                    r.ObjectName = o.ObjectName;
                    r.ObjectImage = o.ObjectImage;
                }
            }

            return reservation;
        }
        public async Task<List<MyReservationDTO>> GetMyReservationHistory(string userId)
        {
            var reservation = await _rekreacijaContext.Set<tbl_Appointment>().Where(s => s.user_id == userId && s.is_approved==true &&s.appointment_date <= DateTime.Now).Select(s => new MyReservationDTO
            {
                AppointmentDate = s.appointment_date,
                AppointmentStartDate = s.start_time,
                AppointmentEndDate = s.end_time,
                object_id = s.object_id,
                is_approved = s.is_approved,
                number_of_players = s.number_of_players,
                price = s.TblPayment.amount
            }).ToListAsync();

            var objectIds = reservation.Select(s => s.object_id).Distinct().ToList();
            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(s => objectIds.Contains(s.id)).Select(s => new
            {
                Id = s.id,
                ObjectName = s.name,
                ObjectAdress = s.address,
                ObjectImage = s.ImagePath
            }).ToListAsync();
            var obj = objects.ToDictionary(o => o.Id, o => o);

            foreach (var r in reservation)
            {
                if (obj.TryGetValue(r.object_id, out var o))
                {
                    r.ObjectAdress = o.ObjectAdress;
                    r.ObjectName = o.ObjectName;
                    r.ObjectImage = o.ObjectImage;
                }
            }

            return reservation;
        }

        public async Task<bool> GetReservedTimes(int objectId, DateTime? startTime, DateTime? endTime)
        {
            var reservation = await _rekreacijaContext.TblAppointment.Where(a => a.object_id == objectId && (a.start_time < endTime && a.end_time > startTime)).ToListAsync();
            if (reservation.Any())
                return true;
            return false;
        }
    }
}