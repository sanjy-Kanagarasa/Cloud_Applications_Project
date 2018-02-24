import { Location } from './driver';
export interface Customer{
  firstName: string,
  lastName: string,
  phoneNumber: string,
  address: Address,
  email: string,
  password: string,
  location: Location;
  userRoleType: number

}
export interface Address{
  streetName: string,
  streetNumber: string,
  zipCode: number
}
