import { FC } from "react";
import { NavLink } from "react-router-dom";

type NavbarProps = {};

const Navbar: FC<NavbarProps> = (props) => {
  return (
    <aside className="w-64 bg-white border-r border-gray-200 shadow-md p-4">
      <h2 className="text-xl font-bold mb-6 text-gray-800">Dashboard</h2>
      <nav className="">
        <NavLink
          to="/emails"
          className={({ isActive }) =>
            `p-4 block font-medium ${
              isActive
                ? "bg-gray-200 text-gray-900"
                : "text-gray-700 hover:bg-gray-100"
            }`
          }
        >
          📧 Emails
        </NavLink>
        <NavLink
          to="/reports"
          className={({ isActive }) =>
            `p-4 block font-medium ${
              isActive
                ? "bg-gray-200 text-gray-900"
                : "text-gray-700 hover:bg-gray-100"
            }`
          }
        >
          📊 Reports
        </NavLink>
      </nav>
    </aside>
  );
};

export default Navbar;
