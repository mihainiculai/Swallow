import React from 'react';
import { FaPlane, FaBed, FaCar, FaBus, FaUtensils, FaGlassCheers, FaLandmark, FaTicketAlt, FaShoppingBag, FaGasPump, FaShoppingCart, FaEllipsisH } from 'react-icons/fa';

type IconProps = {
  id: number;
};

const icons: { [key: number]: JSX.Element } = {
  1: <FaPlane />,
  2: <FaBed />,
  3: <FaCar />,
  4: <FaBus />,
  5: <FaUtensils />,
  6: <FaGlassCheers />,
  7: <FaLandmark />,
  8: <FaTicketAlt />,
  9: <FaShoppingBag />,
  10: <FaGasPump />,
  11: <FaShoppingCart />,
  12: <FaEllipsisH />
};

const ExpenseIcon: React.FC<IconProps> = ({ id }) => {
  return (
    <div>
      {icons[id] || <FaEllipsisH />}
    </div>
  );
};

export default ExpenseIcon;
