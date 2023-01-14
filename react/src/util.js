export const shortenAddress = (address) => {
    if (!address) return "none";
  
    const match = address.match(
      /^(0x[a-zA-Z0-9]{2})[a-zA-Z0-9]+([a-zA-Z0-9]{2})$/
    );
  
    return match ? `${match[1]}...${match[2]}` : address;
  };