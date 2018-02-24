export class Driver{
  firstName: string;
  lastName: string;
  driverID: number;
  phoneNumber: string;
  email: string;
  location: Location;
  distance: number;
  duration: number;
  locationAddress: string;
  totalPrice: number;
  rating: number;
}


export class Location{
  constructor(public latitude: number, public longitude: number) {}
}

export interface FlavourPrice{
  name: string,
  price: number
}
export interface DriverFlavour{
  firstName: string,
  lastName: string,
  driverID: number,
  email: string,
  location: Location,
  flavours: FlavourPrice[]

}
