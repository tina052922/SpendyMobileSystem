import { MapPin, Edit2, Camera } from 'lucide-react';
import StatusBar from '../components/StatusBar';
import BottomNav from '../components/BottomNav';
import imgProfile from 'figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png';

export default function ProfileScreen() {
  const fields = [
    { label: 'Name', value: 'Vilia Crestene' },
    { label: 'Email', value: 'viliagelaga@gmail.com' },
    { label: 'Phone', value: '+63 997 6430394' },
    { label: 'Birthday', value: 'February 21, 2005' },
    { label: 'Gender', value: 'Female' },
  ];

  return (
    <div className="min-h-screen bg-[#E7EBEE] pb-24">
      {/* Header Section */}
      <div className="bg-gradient-to-b from-[#01143D] to-[#0335A3] pb-8">
        <StatusBar />

        <div className="px-6 pt-4">
          <h1 className="text-white text-[24px] font-bold">Profile</h1>
        </div>
      </div>

      {/* Profile Content */}
      <div className="px-6 -mt-4">
        <div className="bg-white rounded-3xl p-6 shadow-sm">
          {/* Profile Photo and Info */}
          <div className="flex items-center gap-4 mb-6 pb-6 border-b border-gray-200">
            <div className="relative">
              <img src={imgProfile} alt="Profile" className="w-20 h-20 rounded-full object-cover" />
              <button className="absolute bottom-0 right-0 w-7 h-7 bg-[#00B2FF] rounded-full flex items-center justify-center shadow-md">
                <Camera className="w-3.5 h-3.5 text-white" />
              </button>
            </div>
            <div>
              <h2 className="text-[20px] font-bold text-[#01143D]">Vilia Crestene</h2>
              <div className="flex items-center gap-2 mt-1">
                <svg className="w-4 h-4" viewBox="0 0 24 24" fill="none">
                  <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z" fill="#E1306C"/>
                  <path d="M12 7c-2.21 0-4 1.79-4 4 0 2.21 1.79 4 4 4s4-1.79 4-4c0-2.21-1.79-4-4-4zm0 6c-1.1 0-2-.9-2-2s.9-2 2-2 2 .9 2 2-.9 2-2 2z" fill="#E1306C"/>
                </svg>
                <span className="text-[#666] text-[13px]">@_crestn</span>
              </div>
            </div>
          </div>

          {/* Profile Fields */}
          <div className="space-y-4">
            {fields.map((field, index) => (
              <div key={index}>
                <label className="text-[#01143D] font-semibold text-[13px] mb-2 block">
                  {field.label}
                </label>
                <div className="flex items-center justify-between bg-[#F5F5F5] px-4 py-3 rounded-xl">
                  <span className="text-[#0335a3] text-[14px]">{field.value}</span>
                  <Edit2 className="w-4 h-4 text-[#0335a3]" />
                </div>
              </div>
            ))}

            {/* Address Field */}
            <div>
              <label className="text-[#01143D] font-semibold text-[13px] mb-2 block">
                Address
              </label>
              <div className="flex items-center justify-between bg-[#F5F5F5] px-4 py-3 rounded-xl">
                <div className="flex items-center gap-2">
                  <MapPin className="w-4 h-4 text-[#0335a3]" />
                  <span className="text-[#0335a3] text-[14px]">Canbanua, Argao, Cebu</span>
                </div>
                <Edit2 className="w-4 h-4 text-[#0335a3]" />
              </div>
            </div>
          </div>
        </div>
      </div>

      <BottomNav />
    </div>
  );
}