import { FC } from "react";
import { Link } from "react-router-dom";

type NavbarProps = {};

const Navbar: FC<NavbarProps> = (props) => {
  return (
    <nav className="h-full flex flex-col items-center justify-center">
      <ul className="">
        <li>
          <Link to={"/emails"}>Emails</Link>
        </li>
        <li>
          <Link to={"/reports"}>Reports</Link>
        </li>
      </ul>
    </nav>
  );
};

export default Navbar;
